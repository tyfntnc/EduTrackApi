using EduTrackApi.Application.Common.Models;
using EduTrackApi.Application.Goals.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackApi.Api.Controllers;

[ApiController]
[Route("v1/goals")]
public sealed class GoalsController : ControllerBase
{
    private static readonly List<GoalDto> MockGoals =
    [
        new()
        {
            Id = "g1", Title = "Fitness Development", Category = "sport", Progress = 75,
            CreatedAt = "2026-04-01T00:00:00Z",
            Tasks =
            [
                new() { Id = "t1", Text = "Training 3 days per week", IsCompleted = true },
                new() { Id = "t2", Text = "10 km running goal", IsCompleted = false },
                new() { Id = "t3", Text = "Flexibility exercises", IsCompleted = false },
                new() { Id = "t4", Text = "Follow nutrition plan", IsCompleted = true }
            ]
        }
    ];

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(ApiResponse<List<GoalDto>>.Ok(MockGoals));
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateGoalRequest request)
    {
        var goal = new GoalDto
        {
            Id = $"g-{Guid.NewGuid():N}",
            Title = request.Title,
            Category = request.Category,
            Progress = 0,
            Tasks = [],
            CreatedAt = DateTime.UtcNow.ToString("o")
        };
        return StatusCode(201, ApiResponse<GoalDto>.Ok(goal));
    }

    [HttpPost("{goalId}/tasks")]
    public IActionResult AddTask(string goalId, [FromBody] AddTaskRequest request)
    {
        var task = new TaskDto { Id = $"t-{Guid.NewGuid():N}", Text = request.Text, IsCompleted = false };
        return StatusCode(201, ApiResponse<TaskDto>.Ok(task));
    }

    [HttpPatch("{goalId}/tasks/{taskId}/toggle")]
    public IActionResult ToggleTask(string goalId, string taskId)
    {
        return Ok(ApiResponse.Ok(new { id = taskId, isCompleted = false, goalProgress = 50 }));
    }

    [HttpDelete("{goalId}")]
    public IActionResult Delete(string goalId)
    {
        return Ok(ApiResponse.Ok(new { message = "Goal and all tasks deleted." }));
    }
}
