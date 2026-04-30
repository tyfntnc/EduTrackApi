namespace EduTrackApi.Application.Analytics.Models;

public sealed class MonthlyActivityDto
{
    public string StudentId { get; init; } = default!;
    public string Month { get; init; } = default!;
    public int TotalLessons { get; init; }
    public int AttendedLessons { get; init; }
    public int MissedLessons { get; init; }
    public double AttendanceRate { get; init; }
    public int TotalHours { get; init; }
    public int ActiveCourseCount { get; init; }
}

public sealed class ProfileCompletionDto
{
    public int Percentage { get; init; }
    public int CompletedCount { get; init; }
    public int TotalCount { get; init; }
    public List<ProfileFieldDto> Fields { get; init; } = [];
}

public sealed class ProfileFieldDto
{
    public string Key { get; init; } = default!;
    public string Label { get; init; } = default!;
    public bool IsCompleted { get; init; }
}

public sealed class SchoolFinancialsDto
{
    public string SchoolId { get; init; } = default!;
    public string Month { get; init; } = default!;
    public int TotalPayments { get; init; }
    public int PaidCount { get; init; }
    public int OverdueCount { get; init; }
    public int PendingCount { get; init; }
    public decimal TotalCollected { get; init; }
    public decimal TotalOverdue { get; init; }
    public double CollectionRate { get; init; }
}
