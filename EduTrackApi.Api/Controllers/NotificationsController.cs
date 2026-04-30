using EduTrackApi.Application.Common.Models;
using EduTrackApi.Application.Notifications.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackApi.Api.Controllers;

[ApiController]
[Route("v1/notifications")]
public sealed class NotificationsController : ControllerBase
{
    private static readonly List<NotificationDto> MockNotifications =
    [
        new() { Id = "n3", Type = "School Announcement", Title = "Weekly Schedule Update", Message = "Training hours have been rearranged.", Timestamp = DateTime.UtcNow.ToString("o"), IsRead = false, SenderRole = "School Admin" },
        new() { Id = "n1", Type = "Class Reminder", Title = "Class Starting", Message = "U19 Football Elite Group class starts in 15 minutes.", Timestamp = DateTime.UtcNow.AddMinutes(-30).ToString("o"), IsRead = false, SenderRole = null },
        new() { Id = "n2", Type = "Attendance Update", Title = "Attendance Recorded", Message = "Mehmet Kaya was marked as \"Present\" in today's mathematics class.", Timestamp = DateTime.UtcNow.AddHours(-1).ToString("o"), IsRead = true, SenderRole = null }
    ];

    [HttpGet]
    public IActionResult GetAll([FromQuery] bool? isRead, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var notifications = MockNotifications.AsEnumerable();
        if (isRead.HasValue)
            notifications = notifications.Where(n => n.IsRead == isRead.Value);
        var list = notifications.ToList();
        return Ok(ApiResponse<List<NotificationDto>>.Ok(list, new MetaData { Page = page, PageSize = pageSize, Total = list.Count, UnreadCount = MockNotifications.Count(n => !n.IsRead) }));
    }

    [HttpPatch("{notificationId}/read")]
    public IActionResult MarkAsRead(string notificationId)
    {
        return Ok(ApiResponse.Ok(new { id = notificationId, isRead = true }));
    }

    [HttpPatch("read-all")]
    public IActionResult MarkAllAsRead()
    {
        return Ok(ApiResponse.Ok(new { updatedCount = MockNotifications.Count }));
    }

    [HttpPost("send")]
    public IActionResult Send([FromBody] SendNotificationRequest request)
    {
        return StatusCode(201, ApiResponse.Ok(new { sentCount = 45, message = "Announcement sent to 45 users." }));
    }

    [HttpPost("payment-reminder")]
    public IActionResult PaymentReminder([FromBody] PaymentReminderRequest request)
    {
        return StatusCode(201, ApiResponse.Ok(new { message = "Payment reminder sent." }));
    }
}
