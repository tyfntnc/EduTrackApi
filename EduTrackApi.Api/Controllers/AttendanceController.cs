using EduTrackApi.Application.Attendance.Models;
using EduTrackApi.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackApi.Api.Controllers;

[Authorize]
[ApiController]
[Route("v1/attendance")]
public sealed class AttendanceController : ControllerBase
{
    [HttpGet("course/{courseId}")]
    public IActionResult GetByCourse(string courseId, [FromQuery] string? month)
    {
        var records = new List<AttendanceRecordDto>
        {
            new() { Id = "att-1", CourseId = courseId, Date = "2026-04-02", PresentStudentIds = ["u2", "u9"], AbsentStudentIds = [] },
            new() { Id = "att-2", CourseId = courseId, Date = "2026-04-07", PresentStudentIds = ["u2"], AbsentStudentIds = ["u9"] }
        };
        return Ok(ApiResponse<List<AttendanceRecordDto>>.Ok(records));
    }

    [HttpGet("student/{studentId}/summary")]
    public IActionResult GetStudentSummary(string studentId, [FromQuery] string? courseId, [FromQuery] string? month)
    {
        var summary = new AttendanceSummaryDto
        {
            StudentId = studentId,
            CourseId = courseId ?? "crs1",
            Month = month ?? "2026-04",
            TotalClasses = 13,
            AttendedClasses = 12,
            MissedClasses = 1,
            AttendanceRate = 92.3,
            DailyRecords =
            [
                new() { Date = "2026-04-02", Status = "present" },
                new() { Date = "2026-04-07", Status = "absent" },
                new() { Date = "2026-04-09", Status = "present" }
            ]
        };
        return Ok(ApiResponse<AttendanceSummaryDto>.Ok(summary));
    }

    [HttpPost]
    public IActionResult Save([FromBody] SaveAttendanceRequest request)
    {
        var record = new AttendanceRecordDto
        {
            Id = $"att-{Guid.NewGuid():N}",
            CourseId = request.CourseId,
            Date = request.Date,
            PresentStudentIds = request.PresentStudentIds,
            AbsentStudentIds = request.AbsentStudentIds,
            RecordedBy = "u1",
            RecordedAt = DateTime.UtcNow.ToString("o")
        };
        return StatusCode(201, ApiResponse<AttendanceRecordDto>.Ok(record));
    }

    [HttpGet("student/{studentId}/weekly")]
    public IActionResult GetWeekly(string studentId)
    {
        var data = new WeeklyActivityDto
        {
            WeekRange = new() { Start = "2026-04-24", End = "2026-04-30" },
            Days =
            [
                new() { Date = "2026-04-24", DayOfWeek = 4, Status = "attended" },
                new() { Date = "2026-04-25", DayOfWeek = 5, Status = "attended" },
                new() { Date = "2026-04-26", DayOfWeek = 6, Status = "none" },
                new() { Date = "2026-04-27", DayOfWeek = 0, Status = "none" },
                new() { Date = "2026-04-28", DayOfWeek = 1, Status = "attended" },
                new() { Date = "2026-04-29", DayOfWeek = 2, Status = "missed" },
                new() { Date = "2026-04-30", DayOfWeek = 3, Status = "attended" }
            ]
        };
        return Ok(ApiResponse<WeeklyActivityDto>.Ok(data));
    }

    [HttpGet("student/{studentId}/monthly-heatmap")]
    public IActionResult GetMonthlyHeatmap(string studentId, [FromQuery] string? month)
    {
        var data = new MonthlyHeatmapDto
        {
            Month = month ?? "2026-04",
            TotalDays = 30,
            ActiveDays = 14,
            AttendanceRate = 92,
            DailyStatuses =
            [
                new() { Day = 1, Status = "none" },
                new() { Day = 2, Status = "attended" },
                new() { Day = 7, Status = "absent" },
                new() { Day = 9, Status = "attended" }
            ]
        };
        return Ok(ApiResponse<MonthlyHeatmapDto>.Ok(data));
    }
}
