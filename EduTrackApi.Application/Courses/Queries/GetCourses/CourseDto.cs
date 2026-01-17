using System;

namespace EduTrackApi.Application.Courses.Queries.GetCourses;

public sealed class CourseDto
{
    public string Id { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string? SchoolId { get; init; }
    public string? BranchId { get; init; }
    public string? CategoryId { get; init; }
    public string? TeacherId { get; init; }

    public string? Location { get; init; }
    public string? Address { get; init; }
}