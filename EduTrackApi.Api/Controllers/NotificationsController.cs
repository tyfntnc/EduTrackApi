using EduTrackApi.Application.Common.Interfaces;
using EduTrackApi.Application.Common.Models;
using EduTrackApi.Application.Notifications.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduTrackApi.Api.Controllers;

[Authorize]
[ApiController]
[Route("v1/notifications")]
public sealed class NotificationsController : ControllerBase
{
    private readonly IEduTrackDbContext _db;

    public NotificationsController(IEduTrackDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool? isRead, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var query = _db.Notifications
            .AsNoTracking()
            .AsQueryable();

        if (isRead.HasValue)
            query = query.Where(n => n.IsRead == isRead.Value);

        var total = await query.CountAsync();
        var unread = await _db.Notifications.CountAsync(n => !n.IsRead);

        var notifications = await query
            .OrderByDescending(n => n.Timestamp)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                Type = n.Type != null ? n.Type.Name : "",
                Title = n.Title,
                Message = n.Message ?? "",
                Timestamp = n.Timestamp.ToString("o"),
                IsRead = n.IsRead,
                SenderRole = n.SenderRole != null ? n.SenderRole.Name : null
            })
            .ToListAsync();

        return Ok(ApiResponse<List<NotificationDto>>.Ok(notifications,
            new MetaData { Page = page, PageSize = pageSize, Total = total, UnreadCount = unread }));
    }

    [HttpPatch("{notificationId}/read")]
    public async Task<IActionResult> MarkAsRead(string notificationId)
    {
        var notification = await _db.Notifications.FindAsync(notificationId);
        if (notification is null) return NotFound(ApiResponse.Fail("NOT_FOUND", "Notification not found."));

        notification.IsRead = true;
        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok(new { id = notificationId, isRead = true }));
    }

    [HttpPatch("read-all")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var unread = await _db.Notifications.Where(n => !n.IsRead).ToListAsync();
        foreach (var n in unread) n.IsRead = true;
        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok(new { updatedCount = unread.Count }));
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] SendNotificationRequest request)
    {
        var notifType = await _db.NotificationTypes.FirstOrDefaultAsync(t => t.Name == request.Type || t.Code == request.Type);

        var notification = new Domain.Entities.Notification
        {
            Id = Guid.NewGuid().ToString("N")[..12],
            TypeId = notifType?.Id ?? 1,
            Title = request.Title,
            Message = request.Message,
            Timestamp = DateTime.UtcNow,
            IsRead = false
        };
        _db.Notifications.Add(notification);
        await _db.SaveChangesAsync();
        return StatusCode(201, ApiResponse.Ok(new { id = notification.Id, message = "Notification sent." }));
    }

    [HttpPost("payment-reminder")]
    public IActionResult PaymentReminder([FromBody] PaymentReminderRequest request)
    {
        // TODO: Implement actual reminder logic
        return StatusCode(201, ApiResponse.Ok(new { message = "Payment reminder sent." }));
    }
}

