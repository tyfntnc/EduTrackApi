using EduTrackApi.Application.Badges.Models;
using EduTrackApi.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackApi.Api.Controllers;

[ApiController]
[Route("v1")]
public sealed class BadgesController : ControllerBase
{
    private static readonly List<BadgeDto> MockBadges =
    [
        new() { Id = "bg1", Name = "7 Day Streak", Description = "You attended training every day for a week. Amazing consistency!", Icon = "\ud83d\udd25", Color = "from-orange-400 to-rose-500", Criteria = new() { Type = "attendance_streak", RequiredDays = 7 } },
        new() { Id = "bg2", Name = "Early Bird", Description = "You are the star of morning trainings before 09:00.", Icon = "\ud83c\udf05", Color = "from-amber-300 to-orange-500", Criteria = new() { Type = "early_attendance", BeforeHour = 9 } },
        new() { Id = "bg3", Name = "Double Shift", Description = "You pushed boundaries by training in two different branches on the same day.", Icon = "\u26a1", Color = "from-indigo-400 to-purple-600" },
        new() { Id = "bg4", Name = "Loyal Athlete", Description = "You reached over 20 hours of training this month.", Icon = "\ud83c\udfaf", Color = "from-emerald-400 to-teal-600" },
        new() { Id = "bg5", Name = "Growth Pioneer", Description = "You received \"Outstanding Success\" comments 3 times in a row from instructor notes.", Icon = "\ud83d\ude80", Color = "from-blue-400 to-indigo-600" },
        new() { Id = "bg6", Name = "Social Butterfly", Description = "You were selected as the most collaborative student in group activities.", Icon = "\ud83e\udd1d", Color = "from-pink-400 to-rose-500" }
    ];

    [HttpGet("badges")]
    public IActionResult GetAll()
    {
        return Ok(ApiResponse<List<BadgeDto>>.Ok(MockBadges));
    }

    [HttpGet("users/{userId}/badges")]
    public IActionResult GetUserBadges(string userId)
    {
        var data = new UserBadgesDto
        {
            Earned =
            [
                new() { Id = "bg1", Name = "7 Day Streak", Icon = "\ud83d\udd25", Color = "from-orange-400 to-rose-500", EarnedAt = "2026-04-15T00:00:00Z" },
                new() { Id = "bg2", Name = "Early Bird", Icon = "\ud83c\udf05", Color = "from-amber-300 to-orange-500", EarnedAt = "2026-04-20T00:00:00Z" }
            ],
            Locked =
            [
                new() { Id = "bg3", Name = "Double Shift", Icon = "\u26a1", Color = "from-indigo-400 to-purple-600" },
                new() { Id = "bg6", Name = "Social Butterfly", Icon = "\ud83e\udd1d", Color = "from-pink-400 to-rose-500" }
            ]
        };
        return Ok(ApiResponse<UserBadgesDto>.Ok(data));
    }

    [HttpPost("users/{userId}/badges")]
    public IActionResult AwardBadge(string userId, [FromBody] AwardBadgeRequest request)
    {
        return Ok(ApiResponse.Ok(new { message = "Badge awarded successfully.", badgeId = request.BadgeId }));
    }
}
