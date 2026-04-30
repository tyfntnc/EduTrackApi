namespace EduTrackApi.Application.Users.Models;

public sealed class UserProfileDto
{
    public string Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Role { get; init; } = default!;
    public string? SchoolId { get; init; }
    public string? Avatar { get; init; }
    public string? PhoneNumber { get; init; }
    public string? BirthDate { get; init; }
    public string? Gender { get; init; }
    public string? Address { get; init; }
    public string? Bio { get; init; }
    public List<string>? BranchIds { get; init; }
    public List<string>? ParentIds { get; init; }
    public List<string>? ChildIds { get; init; }
    public List<string>? Badges { get; init; }
    public int ProfileCompletion { get; init; }
}

public sealed class UpdateProfileRequest
{
    public string? Name { get; init; }
    public string? PhoneNumber { get; init; }
    public string? BirthDate { get; init; }
    public string? Gender { get; init; }
    public string? Address { get; init; }
    public string? Bio { get; init; }
}

public sealed class ChangePasswordRequest
{
    public string CurrentPassword { get; init; } = default!;
    public string NewPassword { get; init; } = default!;
}

public sealed class UpdateRoleRequest
{
    public string Role { get; init; } = default!;
}

public sealed class FamilyMemberDto
{
    public string Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Role { get; init; } = default!;
    public string? Avatar { get; init; }
    public string RelationshipType { get; init; } = default!;
    public string? CustomRoleLabel { get; init; }
}

public sealed class AddFamilyRequest
{
    public string TargetUserId { get; init; } = default!;
    public string RelationshipType { get; init; } = default!;
    public string? CustomRoleLabel { get; init; }
}

public sealed class DeviceTokenRequest
{
    public string Token { get; init; } = default!;
    public string Platform { get; init; } = default!;
}

public sealed class UserListItemDto
{
    public string Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Role { get; init; } = default!;
    public string? Avatar { get; init; }
}
