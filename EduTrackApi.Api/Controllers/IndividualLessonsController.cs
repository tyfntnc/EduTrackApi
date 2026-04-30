using EduTrackApi.Application.Common.Models;
using EduTrackApi.Application.IndividualLessons.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackApi.Api.Controllers;

[ApiController]
[Route("v1/individual-lessons")]
public sealed class IndividualLessonsController : ControllerBase
{
    private static readonly List<IndividualLessonDto> MockLessons =
    [
        new()
        {
            Id = "ind-1", Title = "Private Mathematics", Description = "Preparation study for derivatives topic.",
            Date = "2026-04-30", Time = "14:00", Role = "taken",
            Students = [new() { Name = "Fatma Sahin", Email = "fatma@okul-a.com" }]
        }
    ];

    [HttpGet("my")]
    public IActionResult GetMy([FromQuery] string? date)
    {
        return Ok(ApiResponse<List<IndividualLessonDto>>.Ok(MockLessons));
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateIndividualLessonRequest request)
    {
        var lesson = new IndividualLessonDto
        {
            Id = $"ind-{Guid.NewGuid():N}",
            Title = request.Title,
            Description = request.Description,
            Date = request.Date,
            Time = request.Time,
            Role = request.Role,
            Students = request.Students
        };
        return StatusCode(201, ApiResponse<IndividualLessonDto>.Ok(lesson));
    }

    [HttpDelete("{lessonId}")]
    public IActionResult Delete(string lessonId)
    {
        return Ok(ApiResponse.Ok(new { message = "Lesson deleted." }));
    }
}
