using EduTrackApi.Application.Analytics.Models;
using EduTrackApi.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackApi.Api.Controllers;

[Authorize]
[ApiController]
[Route("v1/analytics")]
public sealed class AnalyticsController : ControllerBase
{
    [HttpGet("students/{studentId}/monthly")]
    public IActionResult GetMonthly(string studentId, [FromQuery] string? month)
    {
        var data = new MonthlyActivityDto
        {
            StudentId = studentId,
            Month = month ?? "2026-04",
            TotalLessons = 14,
            AttendedLessons = 13,
            MissedLessons = 1,
            AttendanceRate = 92.8,
            TotalHours = 28,
            ActiveCourseCount = 2
        };
        return Ok(ApiResponse<MonthlyActivityDto>.Ok(data));
    }

    [HttpGet("users/{userId}/profile-completion")]
    public IActionResult GetProfileCompletion(string userId)
    {
        var data = new ProfileCompletionDto
        {
            Percentage = 87,
            CompletedCount = 7,
            TotalCount = 8,
            Fields =
            [
                new() { Key = "name", Label = "Full Name", IsCompleted = true },
                new() { Key = "email", Label = "Email", IsCompleted = true },
                new() { Key = "phoneNumber", Label = "Phone", IsCompleted = true },
                new() { Key = "avatar", Label = "Photo", IsCompleted = true },
                new() { Key = "birthDate", Label = "Birth Date", IsCompleted = true },
                new() { Key = "gender", Label = "Gender", IsCompleted = true },
                new() { Key = "address", Label = "Address", IsCompleted = true },
                new() { Key = "bio", Label = "About", IsCompleted = false }
            ]
        };
        return Ok(ApiResponse<ProfileCompletionDto>.Ok(data));
    }

    [HttpGet("schools/{schoolId}/financials")]
    public IActionResult GetSchoolFinancials(string schoolId, [FromQuery] string? month)
    {
        var data = new SchoolFinancialsDto
        {
            SchoolId = schoolId,
            Month = month ?? "2026-04",
            TotalPayments = 12,
            PaidCount = 8,
            OverdueCount = 3,
            PendingCount = 1,
            TotalCollected = 9600,
            TotalOverdue = 4500,
            CollectionRate = 68.1
        };
        return Ok(ApiResponse<SchoolFinancialsDto>.Ok(data));
    }
}
