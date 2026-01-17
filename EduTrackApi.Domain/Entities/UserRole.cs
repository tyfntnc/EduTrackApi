using System.Collections.Generic;

namespace EduTrackApi.Domain.Entities;

public class UserRole
{
    public short Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;

    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Notification> NotificationsAsSenderRole { get; set; } = new List<Notification>();
}