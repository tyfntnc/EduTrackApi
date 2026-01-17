using System;

namespace EduTrackApi.Domain.Entities;

public class UserBadge
{
    public string UserId { get; set; } = null!;
    public string BadgeId { get; set; } = null!;
    public DateTime AwardedAt { get; set; }

    public User User { get; set; } = null!;
    public Badge Badge { get; set; } = null!;
}