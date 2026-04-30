namespace EduTrackApi.Application.Badges.Models;

public sealed class BadgeDto
{
    public string Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
    public string Icon { get; init; } = default!;
    public string Color { get; init; } = default!;
    public BadgeCriteriaDto? Criteria { get; init; }
}

public sealed class BadgeCriteriaDto
{
    public string Type { get; init; } = default!;
    public int? RequiredDays { get; init; }
    public int? BeforeHour { get; init; }
}

public sealed class UserBadgesDto
{
    public List<EarnedBadgeDto> Earned { get; init; } = [];
    public List<LockedBadgeDto> Locked { get; init; } = [];
}

public sealed class EarnedBadgeDto
{
    public string Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Icon { get; init; } = default!;
    public string Color { get; init; } = default!;
    public string EarnedAt { get; init; } = default!;
}

public sealed class LockedBadgeDto
{
    public string Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Icon { get; init; } = default!;
    public string Color { get; init; } = default!;
}

public sealed class AwardBadgeRequest
{
    public string BadgeId { get; init; } = default!;
}
