using EduTrackApi.Application.Common.Models;
using EduTrackApi.Application.Courses.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackApi.Api.Controllers;

[ApiController]
[Route("v1/courses")]
public sealed class CoursesV1Controller : ControllerBase
{
    private static readonly List<CourseDetailDto> MockCourses =
    [
        new()
        {
            Id = "crs1", SchoolId = "school-a", BranchId = "b1", BranchName = "Football",
            CategoryId = "c1", CategoryName = "U19", TeacherId = "u1", TeacherName = "Ahmet Yilmaz",
            TeacherAvatar = "https://picsum.photos/seed/u1/200",
            TeacherBio = "15 years of professional football coaching experience.",
            StudentCount = 2, Title = "U19 Football Elite", Location = "Field A",
            Address = "41.0082, 28.9784",
            InstructorNotes = "Please arrive 15 minutes early to start warm-up exercises. Cleats will be checked.",
            Schedule = [new() { Day = 1, StartTime = "16:00", EndTime = "18:00" }, new() { Day = 3, StartTime = "16:00", EndTime = "18:00" }],
            Students = [new() { Id = "u2", Name = "Mehmet Kaya", Avatar = "https://picsum.photos/seed/u2/200", Email = "mehmet@okul-a.com" }, new() { Id = "u9", Name = "Ali Vural", Avatar = "https://picsum.photos/seed/u9/200", Email = "ali@okul-a.com" }],
            UserRole = "student"
        },
        new()
        {
            Id = "crs2", SchoolId = "school-a", BranchId = "b3", BranchName = "Mathematics",
            CategoryId = "c3", CategoryName = "Private Lesson", TeacherId = "u7", TeacherName = "Fatma Sahin",
            TeacherAvatar = "https://picsum.photos/seed/u7/200",
            TeacherBio = "Mathematics Olympiad coordinator.",
            StudentCount = 2, Title = "Mathematics Advanced Level", Location = "Z-12 Laboratory",
            Address = "Ankara, Cankaya",
            InstructorNotes = "Don't forget to bring last week's problem set. We will start the derivatives topic.",
            Schedule = [new() { Day = 2, StartTime = "18:30", EndTime = "20:00" }, new() { Day = 4, StartTime = "18:30", EndTime = "20:00" }],
            Students = [new() { Id = "u2", Name = "Mehmet Kaya", Avatar = "https://picsum.photos/seed/u2/200", Email = "mehmet@okul-a.com" }, new() { Id = "u9", Name = "Ali Vural", Avatar = "https://picsum.photos/seed/u9/200", Email = "ali@okul-a.com" }],
            UserRole = "student"
        }
    ];

    [HttpGet("my")]
    public IActionResult GetMyCourses([FromQuery] string? search)
    {
        var courses = MockCourses.AsEnumerable();
        if (!string.IsNullOrEmpty(search))
            courses = courses.Where(c => c.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
        return Ok(ApiResponse<List<CourseDetailDto>>.Ok(courses.ToList()));
    }

    [HttpGet("{courseId}")]
    public IActionResult GetById(string courseId)
    {
        var course = MockCourses.FirstOrDefault(c => c.Id == courseId);
        if (course is null) return NotFound(ApiResponse.Fail("COURSE_NOT_FOUND", "Course not found."));
        return Ok(ApiResponse<CourseDetailDto>.Ok(course));
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateCourseRequest request)
    {
        var newCourse = new CourseDetailDto
        {
            Id = $"crs-{Guid.NewGuid():N}",
            SchoolId = request.SchoolId,
            BranchId = request.BranchId, BranchName = "Football",
            CategoryId = request.CategoryId, CategoryName = "U19",
            TeacherId = request.TeacherId, TeacherName = "Ahmet Yilmaz",
            TeacherAvatar = "https://picsum.photos/seed/u1/200",
            StudentCount = 0, Title = request.Title,
            Location = request.Location, Address = request.Address,
            InstructorNotes = request.InstructorNotes,
            Schedule = request.Schedule, Students = []
        };
        return StatusCode(201, ApiResponse<CourseDetailDto>.Ok(newCourse));
    }

    [HttpPut("{courseId}")]
    public IActionResult Update(string courseId, [FromBody] CreateCourseRequest request)
    {
        var course = MockCourses.FirstOrDefault(c => c.Id == courseId);
        if (course is null) return NotFound(ApiResponse.Fail("COURSE_NOT_FOUND", "Course not found."));
        return Ok(ApiResponse<CourseDetailDto>.Ok(course));
    }

    [HttpDelete("{courseId}")]
    public IActionResult Delete(string courseId)
    {
        return Ok(ApiResponse.Ok(new { message = "Course deleted." }));
    }

    [HttpPost("{courseId}/students")]
    public IActionResult AddStudent(string courseId, [FromBody] AddStudentRequest request)
    {
        return Ok(ApiResponse.Ok(new { message = "Student added to course.", studentCount = 3 }));
    }

    [HttpDelete("{courseId}/students/{studentId}")]
    public IActionResult RemoveStudent(string courseId, string studentId)
    {
        return Ok(ApiResponse.Ok(new { message = "Student removed from course.", studentCount = 2 }));
    }
}

[ApiController]
[Route("v1/schools/{schoolId}/courses")]
public sealed class SchoolCoursesController : ControllerBase
{
    [HttpGet]
    public IActionResult GetSchoolCourses(string schoolId)
    {
        // Reuse same mock data filtered by schoolId
        var courses = new List<CourseDetailDto>
        {
            new()
            {
                Id = "crs1", SchoolId = schoolId, BranchId = "b1", BranchName = "Football",
                CategoryId = "c1", CategoryName = "U19", TeacherId = "u1", TeacherName = "Ahmet Yilmaz",
                TeacherAvatar = "https://picsum.photos/seed/u1/200",
                StudentCount = 2, Title = "U19 Football Elite", Location = "Field A",
                Address = "41.0082, 28.9784",
                Schedule = [new() { Day = 1, StartTime = "16:00", EndTime = "18:00" }]
            }
        };
        return Ok(ApiResponse<List<CourseDetailDto>>.Ok(courses));
    }
}
