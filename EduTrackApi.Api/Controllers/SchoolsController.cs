using EduTrackApi.Application.Common.Models;
using EduTrackApi.Application.Schools.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackApi.Api.Controllers;

[ApiController]
[Route("v1/schools")]
public sealed class SchoolsController : ControllerBase
{
    private static readonly List<SchoolDto> MockSchools =
    [
        new() { Id = "school-a", Name = "North Star College", Location = "Istanbul, Besiktas", StudentCount = 120, CourseCount = 8, ImageUrl = "https://picsum.photos/seed/school-a/400", CreatedAt = "2024-01-01T00:00:00Z" },
        new() { Id = "school-b", Name = "South Light Sports Academy", Location = "Ankara, Cankaya", StudentCount = 85, CourseCount = 5, ImageUrl = "https://picsum.photos/seed/school-b/400", CreatedAt = "2024-02-15T00:00:00Z" }
    ];

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(ApiResponse<List<SchoolDto>>.Ok(MockSchools, new MetaData { Page = 1, PageSize = 20, Total = MockSchools.Count }));
    }

    [HttpGet("{schoolId}")]
    public IActionResult GetById(string schoolId)
    {
        var school = MockSchools.FirstOrDefault(s => s.Id == schoolId);
        if (school is null) return NotFound(ApiResponse.Fail("SCHOOL_NOT_FOUND", "School not found."));
        return Ok(ApiResponse<SchoolDto>.Ok(school));
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateSchoolRequest request)
    {
        var newSchool = new SchoolDto
        {
            Id = $"school-{Guid.NewGuid():N}",
            Name = request.Name,
            Location = request.Location,
            StudentCount = 0,
            CourseCount = 0,
            ImageUrl = request.ImageUrl,
            CreatedAt = DateTime.UtcNow.ToString("o")
        };
        return StatusCode(201, ApiResponse<SchoolDto>.Ok(newSchool));
    }

    [HttpPut("{schoolId}")]
    public IActionResult Update(string schoolId, [FromBody] UpdateSchoolRequest request)
    {
        var school = MockSchools.FirstOrDefault(s => s.Id == schoolId);
        if (school is null) return NotFound(ApiResponse.Fail("SCHOOL_NOT_FOUND", "School not found."));
        var updated = new SchoolDto { Id = schoolId, Name = request.Name, Location = request.Location, StudentCount = school.StudentCount, CourseCount = school.CourseCount, ImageUrl = request.ImageUrl, CreatedAt = school.CreatedAt };
        return Ok(ApiResponse<SchoolDto>.Ok(updated));
    }

    [HttpDelete("{schoolId}")]
    public IActionResult Delete(string schoolId)
    {
        return Ok(ApiResponse.Ok(new { message = "School and all related data deleted." }));
    }
}
