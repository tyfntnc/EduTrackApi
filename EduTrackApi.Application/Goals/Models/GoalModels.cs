namespace EduTrackApi.Application.Goals.Models;

public sealed class GoalDto
{
    public string Id { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string Category { get; init; } = default!;
    public int Progress { get; init; }
    public List<TaskDto> Tasks { get; init; } = [];
    public string CreatedAt { get; init; } = default!;
}

public sealed class TaskDto
{
    public string Id { get; init; } = default!;
    public string Text { get; init; } = default!;
    public bool IsCompleted { get; set; }
}

public sealed class CreateGoalRequest
{
    public string Title { get; init; } = default!;
    public string Category { get; init; } = default!;
}

public sealed class AddTaskRequest
{
    public string Text { get; init; } = default!;
}
