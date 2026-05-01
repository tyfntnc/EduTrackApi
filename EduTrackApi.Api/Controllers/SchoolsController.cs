using EduTrackApi.Application.Common.Interfaces;
using EduTrackApi.Application.Common.Models;
using EduTrackApi.Application.Schools.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduTrackApi.Api.Controllers;

[Authorize]
[ApiController]
[Route("v1/schools")]
public sealed class SchoolsController : ControllerBase
{
    private readonly IEduTrackDbContext _db;

    public SchoolsController(IEduTrackDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var schools = await _db.Schools
            .Select(s => new SchoolDto
            {
                Id = s.Id,
                Name = s.Name,
                StudentCount = s.Users.Count(u => u.RoleId == 5),
                CourseCount = s.Courses.Count
            })
            .ToListAsync();
        return Ok(ApiResponse<List<SchoolDto>>.Ok(schools, new MetaData { Page = 1, PageSize = 20, Total = schools.Count }));
    }

    [HttpGet("{schoolId}")]
    public async Task<IActionResult> GetById(string schoolId)
    {
        var school = await _db.Schools
            .Select(s => new SchoolDto
            {
                Id = s.Id,
                Name = s.Name,
                StudentCount = s.Users.Count(u => u.RoleId == 5),
                CourseCount = s.Courses.Count
            })
            .FirstOrDefaultAsync(s => s.Id == schoolId);

        if (school is null) return NotFound(ApiResponse.Fail("SCHOOL_NOT_FOUND", "School not found."));
        return Ok(ApiResponse<SchoolDto>.Ok(school));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSchoolRequest request)
    {
        var school = new Domain.Entities.School
        {
            Id = Guid.NewGuid().ToString("N")[..12],
            Name = request.Name
        };
        _db.Schools.Add(school);
        await _db.SaveChangesAsync();

        return StatusCode(201, ApiResponse<SchoolDto>.Ok(new SchoolDto
        {
            Id = school.Id, Name = school.Name,
            StudentCount = 0, CourseCount = 0, CreatedAt = DateTime.UtcNow.ToString("o")
        }));
    }

    [HttpPut("{schoolId}")]
    public async Task<IActionResult> Update(string schoolId, [FromBody] UpdateSchoolRequest request)
    {
        var school = await _db.Schools.FindAsync(schoolId);
        if (school is null) return NotFound(ApiResponse.Fail("SCHOOL_NOT_FOUND", "School not found."));

        school.Name = request.Name;
        await _db.SaveChangesAsync();
        return Ok(ApiResponse<SchoolDto>.Ok(new SchoolDto { Id = school.Id, Name = school.Name }));
    }

    [HttpDelete("{schoolId}")]
    public async Task<IActionResult> Delete(string schoolId)
    {
        var school = await _db.Schools.FindAsync(schoolId);
        if (school is null) return NotFound(ApiResponse.Fail("SCHOOL_NOT_FOUND", "School not found."));

        _db.Schools.Remove(school);
        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok(new { message = "School and all related data deleted." }));
    }
}

