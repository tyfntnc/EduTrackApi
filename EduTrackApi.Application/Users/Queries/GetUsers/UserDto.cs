using System;

namespace EduTrackApi.Application.Users.Queries.GetUsers;

public sealed class UserDto
{
    public string Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string? SchoolId { get; init; }
    public string? RoleCode { get; init; }
}