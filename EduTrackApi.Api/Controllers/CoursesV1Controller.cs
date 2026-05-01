using EduTrackApi.Application.Common.Interfaces;
using EduTrackApi.Application.Common.Models;
using EduTrackApi.Application.Courses.Models;
using EduTrackApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EduTrackApi.Api.Controllers;

[Authorize]
[ApiController]
[Route("v1/courses")]
public sealed class CoursesV1Controller : ControllerBase
{
    private readonly IEduTrackDbContext _db;

    public CoursesV1Controller(IEduTrackDbContext db)
    {
        _db = db;
    }

    private string GetCurrentUserId() =>
        User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub") ?? "";

    [HttpGet("my")]
    public async Task<IActionResult> GetMyCourses([FromQuery] string? search)
    {
        var userId = GetCurrentUserId();

        var query = _db.Courses
            .AsNoTracking()
            .Where(c => c.TeacherId == userId || c.CourseStudents.Any(cs => cs.StudentId == userId));

        if (!string.IsNullOrEmpty(search))
            query = query.Where(c => c.Title.Contains(search));

        var courses = await query
            .Select(c => new CourseDetailDto
            {
                Id = c.Id,
                SchoolId = c.SchoolId ?? "",
                BranchId = c.BranchId ?? "",
                BranchName = c.Branch != null ? c.Branch.Name : "",
                CategoryId = c.CategoryId ?? "",
                CategoryName = c.Category != null ? c.Category.Name : "",
                TeacherId = c.TeacherId ?? "",
                TeacherName = c.Teacher != null ? c.Teacher.Name : "",
                TeacherAvatar = c.Teacher != null ? c.Teacher.Avatar : null,
                TeacherBio = c.Teacher != null ? c.Teacher.Bio : null,
                StudentCount = c.CourseStudents.Count,
                Title = c.Title,
                Location = c.Location,
                Address = c.Address,
                InstructorNotes = c.InstructorNotes,
                Schedule = c.Schedules.Select(s => new ScheduleEntryDto
                {
                    Day = s.DayOfWeek,
                    StartTime = s.StartTime.ToString(@"hh\:mm"),
                    EndTime = s.EndTime.ToString(@"hh\:mm")
                }).ToList(),
                Students = c.CourseStudents.Select(cs => new CourseStudentDto
                {
                    Id = cs.Student != null ? cs.Student.Id : cs.StudentId,
                    Name = cs.Student != null ? cs.Student.Name : "",
                    Avatar = cs.Student != null ? cs.Student.Avatar : null,
                    Email = cs.Student != null ? cs.Student.Email : null
                }).ToList()
            })
            .ToListAsync();

        return Ok(ApiResponse<List<CourseDetailDto>>.Ok(courses));
    }

    [HttpGet("{courseId}")]
    public async Task<IActionResult> GetById(string courseId)
    {
        var course = await _db.Courses
            .AsNoTracking()
            .Where(c => c.Id == courseId)
            .Select(c => new CourseDetailDto
            {
                Id = c.Id,
                SchoolId = c.SchoolId ?? "",
                BranchId = c.BranchId ?? "",
                BranchName = c.Branch != null ? c.Branch.Name : "",
                CategoryId = c.CategoryId ?? "",
                CategoryName = c.Category != null ? c.Category.Name : "",
                TeacherId = c.TeacherId ?? "",
                TeacherName = c.Teacher != null ? c.Teacher.Name : "",
                TeacherAvatar = c.Teacher != null ? c.Teacher.Avatar : null,
                TeacherBio = c.Teacher != null ? c.Teacher.Bio : null,
                StudentCount = c.CourseStudents.Count,
                Title = c.Title,
                Location = c.Location,
                Address = c.Address,
                InstructorNotes = c.InstructorNotes,
                Schedule = c.Schedules.Select(s => new ScheduleEntryDto
                {
                    Day = s.DayOfWeek,
                    StartTime = s.StartTime.ToString(@"hh\:mm"),
                    EndTime = s.EndTime.ToString(@"hh\:mm")
                }).ToList(),
                Students = c.CourseStudents.Select(cs => new CourseStudentDto
                {
                    Id = cs.Student != null ? cs.Student.Id : cs.StudentId,
                    Name = cs.Student != null ? cs.Student.Name : "",
                    Avatar = cs.Student != null ? cs.Student.Avatar : null,
                    Email = cs.Student != null ? cs.Student.Email : null
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (course is null) return NotFound(ApiResponse.Fail("COURSE_NOT_FOUND", "Course not found."));
        return Ok(ApiResponse<CourseDetailDto>.Ok(course));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCourseRequest request)
    {
        var course = new Course
        {
            Id = Guid.NewGuid().ToString("N")[..12],
            SchoolId = request.SchoolId,
            BranchId = request.BranchId,
            CategoryId = request.CategoryId,
            TeacherId = request.TeacherId,
            Title = request.Title,
            Location = request.Location,
            Address = request.Address,
            InstructorNotes = request.InstructorNotes
        };
        _db.Courses.Add(course);

        var schedId = (await _db.CourseSchedules.MaxAsync(s => (int?)s.Id) ?? 0);
        foreach (var s in request.Schedule)
        {
            _db.CourseSchedules.Add(new CourseSchedule
            {
                Id = ++schedId,
                CourseId = course.Id,
                DayOfWeek = (short)s.Day,
                StartTime = TimeSpan.Parse(s.StartTime),
                EndTime = TimeSpan.Parse(s.EndTime)
            });
        }

        await _db.SaveChangesAsync();

        var created = await _db.Courses
            .Include(c => c.Branch).Include(c => c.Category)
            .Include(c => c.Teacher).Include(c => c.Schedules)
            .Include(c => c.CourseStudents).ThenInclude(cs => cs.Student)
            .FirstAsync(c => c.Id == course.Id);

        return StatusCode(201, ApiResponse<CourseDetailDto>.Ok(MapToDto(created)));
    }

    [HttpPut("{courseId}")]
    public async Task<IActionResult> Update(string courseId, [FromBody] CreateCourseRequest request)
    {
        var course = await _db.Courses.Include(c => c.Schedules).FirstOrDefaultAsync(c => c.Id == courseId);
        if (course is null) return NotFound(ApiResponse.Fail("COURSE_NOT_FOUND", "Course not found."));

        course.Title = request.Title;
        course.BranchId = request.BranchId;
        course.CategoryId = request.CategoryId;
        course.TeacherId = request.TeacherId;
        course.Location = request.Location;
        course.Address = request.Address;
        course.InstructorNotes = request.InstructorNotes;

        _db.CourseSchedules.RemoveRange(course.Schedules);
        var schedId = (await _db.CourseSchedules.MaxAsync(s => (int?)s.Id) ?? 0);
        foreach (var s in request.Schedule)
        {
            _db.CourseSchedules.Add(new CourseSchedule
            {
                Id = ++schedId,
                CourseId = course.Id,
                DayOfWeek = (short)s.Day,
                StartTime = TimeSpan.Parse(s.StartTime),
                EndTime = TimeSpan.Parse(s.EndTime)
            });
        }

        await _db.SaveChangesAsync();

        var updated = await _db.Courses
            .Include(c => c.Branch).Include(c => c.Category)
            .Include(c => c.Teacher).Include(c => c.Schedules)
            .Include(c => c.CourseStudents).ThenInclude(cs => cs.Student)
            .FirstAsync(c => c.Id == courseId);

        return Ok(ApiResponse<CourseDetailDto>.Ok(MapToDto(updated)));
    }

    [HttpDelete("{courseId}")]
    public async Task<IActionResult> Delete(string courseId)
    {
        var course = await _db.Courses.FindAsync(courseId);
        if (course is null) return NotFound(ApiResponse.Fail("COURSE_NOT_FOUND", "Course not found."));

        _db.Courses.Remove(course);
        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok(new { message = "Course deleted." }));
    }

    [HttpPost("{courseId}/students")]
    public async Task<IActionResult> AddStudent(string courseId, [FromBody] AddStudentRequest request)
    {
        _db.CourseStudents.Add(new CourseStudent { CourseId = courseId, StudentId = request.StudentId });
        await _db.SaveChangesAsync();
        var count = await _db.CourseStudents.CountAsync(cs => cs.CourseId == courseId);
        return Ok(ApiResponse.Ok(new { message = "Student added to course.", studentCount = count }));
    }

    [HttpDelete("{courseId}/students/{studentId}")]
    public async Task<IActionResult> RemoveStudent(string courseId, string studentId)
    {
        var cs = await _db.CourseStudents.FindAsync(courseId, studentId);
        if (cs is not null) _db.CourseStudents.Remove(cs);
        await _db.SaveChangesAsync();
        var count = await _db.CourseStudents.CountAsync(x => x.CourseId == courseId);
        return Ok(ApiResponse.Ok(new { message = "Student removed from course.", studentCount = count }));
    }

    private static CourseDetailDto MapToDto(Course c) => new()
    {
        Id = c.Id,
        SchoolId = c.SchoolId ?? "",
        BranchId = c.BranchId ?? "",
        BranchName = c.Branch?.Name ?? "",
        CategoryId = c.CategoryId ?? "",
        CategoryName = c.Category?.Name ?? "",
        TeacherId = c.TeacherId ?? "",
        TeacherName = c.Teacher?.Name ?? "",
        TeacherAvatar = c.Teacher?.Avatar,
        TeacherBio = c.Teacher?.Bio,
        StudentCount = c.CourseStudents.Count,
        Title = c.Title,
        Location = c.Location,
        Address = c.Address,
        InstructorNotes = c.InstructorNotes,
        Schedule = c.Schedules.Select(s => new ScheduleEntryDto
        {
            Day = s.DayOfWeek,
            StartTime = s.StartTime.ToString(@"hh\:mm"),
            EndTime = s.EndTime.ToString(@"hh\:mm")
        }).ToList(),
        Students = c.CourseStudents.Select(cs => new CourseStudentDto
        {
            Id = cs.Student?.Id ?? cs.StudentId,
            Name = cs.Student?.Name ?? "",
            Avatar = cs.Student?.Avatar,
            Email = cs.Student?.Email
        }).ToList()
    };
}

[Authorize]
[ApiController]
[Route("v1/schools/{schoolId}/courses")]
public sealed class SchoolCoursesController : ControllerBase
{
    private readonly IEduTrackDbContext _db;

    public SchoolCoursesController(IEduTrackDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetSchoolCourses(string schoolId)
    {
        var courses = await _db.Courses
            .AsNoTracking()
            .Where(c => c.SchoolId == schoolId)
            .Select(c => new CourseDetailDto
            {
                Id = c.Id,
                SchoolId = c.SchoolId ?? "",
                BranchId = c.BranchId ?? "",
                BranchName = c.Branch != null ? c.Branch.Name : "",
                CategoryId = c.CategoryId ?? "",
                CategoryName = c.Category != null ? c.Category.Name : "",
                TeacherId = c.TeacherId ?? "",
                TeacherName = c.Teacher != null ? c.Teacher.Name : "",
                TeacherAvatar = c.Teacher != null ? c.Teacher.Avatar : null,
                StudentCount = c.CourseStudents.Count,
                Title = c.Title,
                Location = c.Location,
                Address = c.Address,
                Schedule = c.Schedules.Select(s => new ScheduleEntryDto
                {
                    Day = s.DayOfWeek,
                    StartTime = s.StartTime.ToString(@"hh\:mm"),
                    EndTime = s.EndTime.ToString(@"hh\:mm")
                }).ToList()
            })
            .ToListAsync();

        return Ok(ApiResponse<List<CourseDetailDto>>.Ok(courses));
    }
}

