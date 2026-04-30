namespace EduTrackApi.Application.Schools.Models;

public sealed class SchoolDto
{
    public string Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Location { get; init; } = default!;
    public int StudentCount { get; init; }
    public int CourseCount { get; init; }
    public string? ImageUrl { get; init; }
    public string? CreatedAt { get; init; }
}

public sealed class CreateSchoolRequest
{
    public string Name { get; init; } = default!;
    public string Location { get; init; } = default!;
    public string? ImageUrl { get; init; }
}

public sealed class UpdateSchoolRequest
{
    public string Name { get; init; } = default!;
    public string Location { get; init; } = default!;
    public string? ImageUrl { get; init; }
}
