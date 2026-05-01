using EduTrackApi.Application.Common.Interfaces;
using EduTrackApi.Application.Common.Models;
using EduTrackApi.Application.Users.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EduTrackApi.Api.Controllers;

[Authorize]
[ApiController]
[Route("v1/users")]
public sealed class UsersV1Controller : ControllerBase
{
    private readonly IEduTrackDbContext _db;

    public UsersV1Controller(IEduTrackDbContext db)
    {
        _db = db;
    }

    private string GetCurrentUserId() =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? User.FindFirstValue("sub")
        ?? throw new UnauthorizedAccessException();

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = GetCurrentUserId();
        var profile = await _db.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new UserProfileDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role != null ? u.Role.Code : "",
                SchoolId = u.SchoolId,
                Avatar = u.Avatar,
                PhoneNumber = u.PhoneNumber,
                BirthDate = u.BirthDate.HasValue ? u.BirthDate.Value.ToString("yyyy-MM-dd") : null,
                Gender = u.Gender,
                Address = u.Address,
                Bio = u.Bio,
                ParentIds = u.ParentChildrenAsChild.Select(pc => pc.ParentId).ToList(),
                ChildIds = u.ParentChildrenAsParent.Select(pc => pc.ChildId).ToList(),
                Badges = u.UserBadges.Select(ub => ub.BadgeId).ToList(),
                BranchIds = u.CourseStudents.Select(cs => cs.Course!.BranchId).Where(b => b != null).Distinct().ToList()!,
                ProfileCompletion = (int)Math.Round(
                    ((u.Name != null && u.Name != "" ? 1 : 0) +
                     (u.Email != null && u.Email != "" ? 1 : 0) +
                     (u.PhoneNumber != null && u.PhoneNumber != "" ? 1 : 0) +
                     (u.Avatar != null && u.Avatar != "" ? 1 : 0) +
                     (u.BirthDate.HasValue ? 1 : 0) +
                     (u.Gender != null && u.Gender != "" ? 1 : 0) +
                     (u.Address != null && u.Address != "" ? 1 : 0) +
                     (u.Bio != null && u.Bio != "" ? 1 : 0)) * 100.0 / 8)
            })
            .FirstOrDefaultAsync();

        if (profile is null) return NotFound(ApiResponse.Fail("USER_NOT_FOUND", "User not found."));
        return Ok(ApiResponse<UserProfileDto>.Ok(profile));
    }

    [HttpPatch("me")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = GetCurrentUserId();
        var user = await _db.Users.FindAsync(userId);
        if (user is null) return NotFound(ApiResponse.Fail("USER_NOT_FOUND", "User not found."));

        if (request.Name is not null) user.Name = request.Name;
        if (request.PhoneNumber is not null) user.PhoneNumber = request.PhoneNumber;
        if (request.BirthDate is not null && DateTime.TryParse(request.BirthDate, out var bd))
            user.BirthDate = DateTime.SpecifyKind(bd, DateTimeKind.Utc);
        if (request.Gender is not null) user.Gender = request.Gender;
        if (request.Address is not null) user.Address = request.Address;
        if (request.Bio is not null) user.Bio = request.Bio;

        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok(new { id = user.Id, message = "Profile updated." }));
    }

    [HttpPatch("me/password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = GetCurrentUserId();
        var user = await _db.Users.FindAsync(userId);
        if (user is null) return NotFound(ApiResponse.Fail("USER_NOT_FOUND", "User not found."));

        if (string.IsNullOrEmpty(user.PasswordHash) || !BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            return BadRequest(ApiResponse.Fail("WRONG_PASSWORD", "Current password is incorrect."));

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok(new { message = "Password updated." }));
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetById(string userId)
    {
        var profile = await _db.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new UserProfileDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role != null ? u.Role.Code : "",
                SchoolId = u.SchoolId,
                Avatar = u.Avatar,
                PhoneNumber = u.PhoneNumber,
                BirthDate = u.BirthDate.HasValue ? u.BirthDate.Value.ToString("yyyy-MM-dd") : null,
                Gender = u.Gender,
                Address = u.Address,
                Bio = u.Bio,
                ParentIds = u.ParentChildrenAsChild.Select(pc => pc.ParentId).ToList(),
                ChildIds = u.ParentChildrenAsParent.Select(pc => pc.ChildId).ToList(),
                Badges = u.UserBadges.Select(ub => ub.BadgeId).ToList(),
                BranchIds = u.CourseStudents.Select(cs => cs.Course!.BranchId).Where(b => b != null).Distinct().ToList()!,
                ProfileCompletion = (int)Math.Round(
                    ((u.Name != null && u.Name != "" ? 1 : 0) +
                     (u.Email != null && u.Email != "" ? 1 : 0) +
                     (u.PhoneNumber != null && u.PhoneNumber != "" ? 1 : 0) +
                     (u.Avatar != null && u.Avatar != "" ? 1 : 0) +
                     (u.BirthDate.HasValue ? 1 : 0) +
                     (u.Gender != null && u.Gender != "" ? 1 : 0) +
                     (u.Address != null && u.Address != "" ? 1 : 0) +
                     (u.Bio != null && u.Bio != "" ? 1 : 0)) * 100.0 / 8)
            })
            .FirstOrDefaultAsync();

        if (profile is null) return NotFound(ApiResponse.Fail("USER_NOT_FOUND", "User not found."));
        return Ok(ApiResponse<UserProfileDto>.Ok(profile));
    }

    [HttpPatch("{userId}/role")]
    public async Task<IActionResult> UpdateRole(string userId, [FromBody] UpdateRoleRequest request)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user is null) return NotFound(ApiResponse.Fail("USER_NOT_FOUND", "User not found."));

        var role = await _db.UserRoles.FirstOrDefaultAsync(r => r.Code == request.Role);
        if (role is null) return BadRequest(ApiResponse.Fail("INVALID_ROLE", "Role not found."));

        user.RoleId = role.Id;
        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok(new { id = userId, role = role.Code, updatedAt = DateTime.UtcNow.ToString("o") }));
    }

    [HttpGet("me/family")]
    public async Task<IActionResult> GetFamily()
    {
        var userId = GetCurrentUserId();
        var user = await _db.Users
            .Include(u => u.ParentChildrenAsChild).ThenInclude(pc => pc.Parent).ThenInclude(p => p.Role)
            .Include(u => u.ParentChildrenAsParent).ThenInclude(pc => pc.Child).ThenInclude(c => c.Role)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null) return NotFound(ApiResponse.Fail("USER_NOT_FOUND", "User not found."));

        var family = new List<FamilyMemberDto>();
        foreach (var pc in user.ParentChildrenAsChild)
        {
            family.Add(new FamilyMemberDto
            {
                Id = pc.Parent.Id, Name = pc.Parent.Name, Email = pc.Parent.Email,
                Role = pc.Parent.Role?.Code ?? "", Avatar = pc.Parent.Avatar,
                RelationshipType = "parent"
            });
        }
        foreach (var pc in user.ParentChildrenAsParent)
        {
            family.Add(new FamilyMemberDto
            {
                Id = pc.Child.Id, Name = pc.Child.Name, Email = pc.Child.Email,
                Role = pc.Child.Role?.Code ?? "", Avatar = pc.Child.Avatar,
                RelationshipType = "child"
            });
        }
        return Ok(ApiResponse<List<FamilyMemberDto>>.Ok(family));
    }

    [HttpPost("me/family")]
    public async Task<IActionResult> AddFamily([FromBody] AddFamilyRequest request)
    {
        var userId = GetCurrentUserId();
        var target = await _db.Users.FindAsync(request.TargetUserId);
        if (target is null) return NotFound(ApiResponse.Fail("USER_NOT_FOUND", "Target user not found."));

        if (request.RelationshipType == "parent")
            _db.ParentChildren.Add(new Domain.Entities.ParentChild { ParentId = request.TargetUserId, ChildId = userId });
        else
            _db.ParentChildren.Add(new Domain.Entities.ParentChild { ParentId = userId, ChildId = request.TargetUserId });

        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok(new { message = "Family connection added." }));
    }

    [HttpDelete("me/family/{targetUserId}")]
    public async Task<IActionResult> RemoveFamily(string targetUserId)
    {
        var userId = GetCurrentUserId();
        var link = await _db.ParentChildren
            .FirstOrDefaultAsync(pc =>
                (pc.ParentId == userId && pc.ChildId == targetUserId) ||
                (pc.ParentId == targetUserId && pc.ChildId == userId));

        if (link is null) return NotFound(ApiResponse.Fail("NOT_FOUND", "Family connection not found."));

        _db.ParentChildren.Remove(link);
        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok(new { message = "Family connection removed." }));
    }

    private static UserProfileDto MapToProfile(Domain.Entities.User user)
    {
        var filledFields = 0;
        var totalFields = 8;
        if (!string.IsNullOrEmpty(user.Name)) filledFields++;
        if (!string.IsNullOrEmpty(user.Email)) filledFields++;
        if (!string.IsNullOrEmpty(user.PhoneNumber)) filledFields++;
        if (!string.IsNullOrEmpty(user.Avatar)) filledFields++;
        if (user.BirthDate.HasValue) filledFields++;
        if (!string.IsNullOrEmpty(user.Gender)) filledFields++;
        if (!string.IsNullOrEmpty(user.Address)) filledFields++;
        if (!string.IsNullOrEmpty(user.Bio)) filledFields++;

        return new UserProfileDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role?.Code ?? "",
            SchoolId = user.SchoolId,
            Avatar = user.Avatar,
            PhoneNumber = user.PhoneNumber,
            BirthDate = user.BirthDate?.ToString("yyyy-MM-dd"),
            Gender = user.Gender,
            Address = user.Address,
            Bio = user.Bio,
            ParentIds = user.ParentChildrenAsChild.Select(pc => pc.ParentId).ToList(),
            ChildIds = user.ParentChildrenAsParent.Select(pc => pc.ChildId).ToList(),
            Badges = user.UserBadges.Select(ub => ub.BadgeId).ToList(),
            BranchIds = user.CourseStudents.Select(cs => cs.Course?.BranchId).Where(b => b is not null).Distinct().ToList()!,
            ProfileCompletion = (int)Math.Round(filledFields * 100.0 / totalFields)
        };
    }
}

[Authorize]
[ApiController]
[Route("v1/schools/{schoolId}/users")]
public sealed class SchoolUsersController : ControllerBase
{
    private readonly IEduTrackDbContext _db;

    public SchoolUsersController(IEduTrackDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetSchoolUsers(string schoolId, [FromQuery] string? role, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var query = _db.Users.Include(u => u.Role).Where(u => u.SchoolId == schoolId);

        if (!string.IsNullOrEmpty(role))
            query = query.Where(u => u.Role.Code == role);

        var total = await query.CountAsync();
        var users = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserListItemDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role.Code,
                Avatar = u.Avatar
            })
            .ToListAsync();

        return Ok(ApiResponse<List<UserListItemDto>>.Ok(users, new MetaData { Page = page, PageSize = pageSize, Total = total }));
    }
}

