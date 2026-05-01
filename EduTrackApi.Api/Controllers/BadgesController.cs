using EduTrackApi.Application.Badges.Models;
using EduTrackApi.Application.Common.Interfaces;
using EduTrackApi.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduTrackApi.Api.Controllers;

[Authorize]
[ApiController]
[Route("v1")]
public sealed class BadgesController : ControllerBase
{
    private readonly IEduTrackDbContext _db;

    public BadgesController(IEduTrackDbContext db)
    {
        _db = db;
    }

    [HttpGet("badges")]
    public async Task<IActionResult> GetAll()
    {
        var badges = await _db.Badges
            .Select(b => new BadgeDto
            {
                Id = b.Id, Name = b.Name, Description = b.Description,
                Icon = b.Icon, Color = b.Color
            })
            .ToListAsync();
        return Ok(ApiResponse<List<BadgeDto>>.Ok(badges));
    }

    [HttpGet("users/{userId}/badges")]
    public async Task<IActionResult> GetUserBadges(string userId)
    {
        var allBadges = await _db.Badges.ToListAsync();
        var earnedIds = await _db.UserBadges
            .Where(ub => ub.UserId == userId)
            .ToListAsync();

        var earnedBadgeIds = earnedIds.Select(ub => ub.BadgeId).ToHashSet();

        var data = new UserBadgesDto
        {
            Earned = allBadges.Where(b => earnedBadgeIds.Contains(b.Id)).Select(b => new EarnedBadgeDto
            {
                Id = b.Id, Name = b.Name, Icon = b.Icon, Color = b.Color,
                EarnedAt = earnedIds.First(ub => ub.BadgeId == b.Id).AwardedAt.ToString("o")
            }).ToList(),
            Locked = allBadges.Where(b => !earnedBadgeIds.Contains(b.Id)).Select(b => new LockedBadgeDto
            {
                Id = b.Id, Name = b.Name, Icon = b.Icon, Color = b.Color
            }).ToList()
        };
        return Ok(ApiResponse<UserBadgesDto>.Ok(data));
    }

    [HttpPost("users/{userId}/badges")]
    public async Task<IActionResult> AwardBadge(string userId, [FromBody] AwardBadgeRequest request)
    {
        var exists = await _db.UserBadges.AnyAsync(ub => ub.UserId == userId && ub.BadgeId == request.BadgeId);
        if (exists) return Conflict(ApiResponse.Fail("ALREADY_AWARDED", "Badge already awarded."));

        _db.UserBadges.Add(new Domain.Entities.UserBadge
        {
            UserId = userId,
            BadgeId = request.BadgeId,
            AwardedAt = DateTime.UtcNow
        });
        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok(new { message = "Badge awarded successfully.", badgeId = request.BadgeId }));
    }
}

