using System.Collections.Generic;

namespace EduTrackApi.Domain.Entities;

public class NotificationType
{
    public short Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;

    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}