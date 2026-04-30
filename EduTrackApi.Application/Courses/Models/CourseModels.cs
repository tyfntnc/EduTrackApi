namespace EduTrackApi.Application.Courses.Models;

public sealed class CourseDetailDto
{
    public string Id { get; init; } = default!;
    public string SchoolId { get; init; } = default!;
    public string BranchId { get; init; } = default!;
    public string BranchName { get; init; } = default!;
    public string CategoryId { get; init; } = default!;
    public string CategoryName { get; init; } = default!;
    public string TeacherId { get; init; } = default!;
    public string TeacherName { get; init; } = default!;
    public string? TeacherAvatar { get; init; }
    public string? TeacherBio { get; init; }
    public int StudentCount { get; init; }
    public string Title { get; init; } = default!;
    public string? Location { get; init; }
    public string? Address { get; init; }
    public string? InstructorNotes { get; init; }
    public List<ScheduleEntryDto> Schedule { get; init; } = [];
    public List<CourseStudentDto>? Students { get; init; }
    public string? UserRole { get; init; }
}

public sealed class ScheduleEntryDto
{
    public int Day { get; init; }
    public string StartTime { get; init; } = default!;
    public string EndTime { get; init; } = default!;
}

public sealed class CourseStudentDto
{
    public string Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string? Avatar { get; init; }
    public string? Email { get; init; }
}

public sealed class CreateCourseRequest
{
    public string SchoolId { get; init; } = default!;
    public string BranchId { get; init; } = default!;
    public string CategoryId { get; init; } = default!;
    public string TeacherId { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string? Location { get; init; }
    public string? Address { get; init; }
    public string? InstructorNotes { get; init; }
    public List<ScheduleEntryDto> Schedule { get; init; } = [];
}

public sealed class AddStudentRequest
{
    public string StudentId { get; init; } = default!;
}
