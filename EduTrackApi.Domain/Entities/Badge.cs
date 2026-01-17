using System.Collections.Generic;

namespace EduTrackApi.Domain.Entities;

public class Badge
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Icon { get; set; } = null!;
    public string Color { get; set; } = null!;

    public ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
}