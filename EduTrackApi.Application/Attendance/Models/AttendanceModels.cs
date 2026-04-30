namespace EduTrackApi.Application.Attendance.Models;

public sealed class AttendanceRecordDto
{
    public string Id { get; init; } = default!;
    public string CourseId { get; init; } = default!;
    public string Date { get; init; } = default!;
    public List<string> PresentStudentIds { get; init; } = [];
    public List<string> AbsentStudentIds { get; init; } = [];
    public string? RecordedBy { get; init; }
    public string? RecordedAt { get; init; }
}

public sealed class SaveAttendanceRequest
{
    public string CourseId { get; init; } = default!;
    public string Date { get; init; } = default!;
    public List<string> PresentStudentIds { get; init; } = [];
    public List<string> AbsentStudentIds { get; init; } = [];
}

public sealed class AttendanceSummaryDto
{
    public string StudentId { get; init; } = default!;
    public string CourseId { get; init; } = default!;
    public string Month { get; init; } = default!;
    public int TotalClasses { get; init; }
    public int AttendedClasses { get; init; }
    public int MissedClasses { get; init; }
    public double AttendanceRate { get; init; }
    public List<DailyRecordDto> DailyRecords { get; init; } = [];
}

public sealed class DailyRecordDto
{
    public string Date { get; init; } = default!;
    public string Status { get; init; } = default!;
}

public sealed class WeeklyActivityDto
{
    public WeekRangeDto WeekRange { get; init; } = default!;
    public List<WeekDayDto> Days { get; init; } = [];
}

public sealed class WeekRangeDto
{
    public string Start { get; init; } = default!;
    public string End { get; init; } = default!;
}

public sealed class WeekDayDto
{
    public string Date { get; init; } = default!;
    public int DayOfWeek { get; init; }
    public string Status { get; init; } = default!;
}

public sealed class MonthlyHeatmapDto
{
    public string Month { get; init; } = default!;
    public int TotalDays { get; init; }
    public int ActiveDays { get; init; }
    public double AttendanceRate { get; init; }
    public List<DailyStatusDto> DailyStatuses { get; init; } = [];
}

public sealed class DailyStatusDto
{
    public int Day { get; init; }
    public string Status { get; init; } = default!;
}
