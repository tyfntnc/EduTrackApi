using System;

namespace EduTrackApi.Domain.Entities;

public class Notification
{
    public string Id { get; set; } = null!;
    public short TypeId { get; set; }
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public DateTime Timestamp { get; set; }
    public bool IsRead { get; set; }
    public short? SenderRoleId { get; set; }

    public NotificationType Type { get; set; } = null!;
    public UserRole? SenderRole { get; set; }
}