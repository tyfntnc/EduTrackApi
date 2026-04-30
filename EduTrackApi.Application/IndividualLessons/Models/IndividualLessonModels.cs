namespace EduTrackApi.Application.IndividualLessons.Models;

public sealed class IndividualLessonDto
{
    public string Id { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string? Description { get; init; }
    public string Date { get; init; } = default!;
    public string Time { get; init; } = default!;
    public string Role { get; init; } = default!;
    public List<LessonParticipantDto> Students { get; init; } = [];
}

public sealed class LessonParticipantDto
{
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
}

public sealed class CreateIndividualLessonRequest
{
    public string Title { get; init; } = default!;
    public string? Description { get; init; }
    public string Date { get; init; } = default!;
    public string Time { get; init; } = default!;
    public string Role { get; init; } = default!;
    public List<LessonParticipantDto> Students { get; init; } = [];
}
