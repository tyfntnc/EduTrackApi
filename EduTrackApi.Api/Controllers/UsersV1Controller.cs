using EduTrackApi.Application.Common.Models;
using EduTrackApi.Application.Users.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackApi.Api.Controllers;

[ApiController]
[Route("v1/users")]
public sealed class UsersV1Controller : ControllerBase
{
    private static readonly List<UserProfileDto> MockUsers =
    [
        new() { Id = "admin", Name = "Zeynep Sistem", Email = "admin@edutrack.com", Role = "System Admin", SchoolId = null, Avatar = "https://picsum.photos/seed/admin/200", PhoneNumber = "0555 111 22 33", Gender = "Female", BirthDate = "1990-05-15", Address = "Istanbul, Turkey", Badges = [], ProfileCompletion = 75 },
        new() { Id = "u4", Name = "Canan Sert", Email = "canan@okul-a.com", Role = "School Admin", SchoolId = "school-a", Avatar = "https://picsum.photos/seed/u4/200", PhoneNumber = "0555 444 55 66", ProfileCompletion = 60 },
        new() { Id = "u1", Name = "Ahmet Yilmaz", Email = "ahmet@okul-a.com", Role = "Teacher/Coach", SchoolId = "school-a", Avatar = "https://picsum.photos/seed/u1/200", Bio = "15 years of professional football coaching experience.", PhoneNumber = "0532 123 45 67", ProfileCompletion = 90 },
        new() { Id = "u3", Name = "Ayse Demir", Email = "ayse@veli.com", Role = "Parent", Avatar = "https://picsum.photos/seed/u3/200", ChildIds = ["u2", "u9"], ProfileCompletion = 50 },
        new() { Id = "u2", Name = "Mehmet Kaya", Email = "mehmet@okul-a.com", Role = "Student", SchoolId = "school-a", Avatar = "https://picsum.photos/seed/u2/200", ParentIds = ["u3"], Badges = ["bg1", "bg2", "bg3", "bg4", "bg5"], PhoneNumber = "0505 987 65 43", BirthDate = "2008-10-12", Gender = "Male", Address = "Ankara, Turkey", BranchIds = ["b1", "b3"], ProfileCompletion = 87 },
        new() { Id = "u9", Name = "Ali Vural", Email = "ali@okul-a.com", Role = "Student", SchoolId = "school-a", Avatar = "https://picsum.photos/seed/u9/200", ParentIds = ["u3"], Badges = ["bg1"], ProfileCompletion = 40 },
        new() { Id = "u5", Name = "Bulent Arin", Email = "bulent@okul-b.com", Role = "School Admin", SchoolId = "school-b", Avatar = "https://picsum.photos/seed/u5/200", ProfileCompletion = 55 },
        new() { Id = "u7", Name = "Fatma Sahin", Email = "fatma@okul-a.com", Role = "Teacher/Coach", SchoolId = "school-a", Avatar = "https://picsum.photos/seed/u7/200", Bio = "Mathematics Olympiad coordinator.", ProfileCompletion = 70 },
        new() { Id = "u8", Name = "Murat Can", Email = "murat@okul-a.com", Role = "Teacher/Coach", SchoolId = "school-a", Avatar = "https://picsum.photos/seed/u8/200", ProfileCompletion = 45 }
    ];

    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        // Mock: u2 is the current session user
        var user = MockUsers.First(u => u.Id == "u2");
        return Ok(ApiResponse<UserProfileDto>.Ok(user));
    }

    [HttpPatch("me")]
    public IActionResult UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var user = MockUsers.First(u => u.Id == "u2");
        return Ok(ApiResponse.Ok(new
        {
            id = user.Id,
            name = request.Name ?? user.Name,
            phoneNumber = request.PhoneNumber ?? user.PhoneNumber,
            birthDate = request.BirthDate ?? user.BirthDate,
            gender = request.Gender ?? user.Gender,
            address = request.Address ?? user.Address,
            bio = request.Bio ?? user.Bio,
            profileCompletion = 100
        }));
    }

    [HttpPost("me/avatar")]
    public IActionResult UploadAvatar()
    {
        return Ok(ApiResponse.Ok(new { avatarUrl = "https://picsum.photos/seed/u2-new/200" }));
    }

    [HttpPatch("me/password")]
    public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
    {
        return Ok(ApiResponse.Ok(new { message = "Password updated." }));
    }

    [HttpGet("{userId}")]
    public IActionResult GetById(string userId)
    {
        var user = MockUsers.FirstOrDefault(u => u.Id == userId);
        if (user is null) return NotFound(ApiResponse.Fail("USER_NOT_FOUND", "User not found."));
        return Ok(ApiResponse<UserProfileDto>.Ok(user));
    }

    [HttpPatch("{userId}/role")]
    public IActionResult UpdateRole(string userId, [FromBody] UpdateRoleRequest request)
    {
        return Ok(ApiResponse.Ok(new { id = userId, role = request.Role, updatedAt = DateTime.UtcNow.ToString("o") }));
    }

    [HttpGet("me/family")]
    public IActionResult GetFamily()
    {
        var family = new List<FamilyMemberDto>
        {
            new() { Id = "u3", Name = "Ayse Demir", Email = "ayse@veli.com", Role = "Parent", Avatar = "https://picsum.photos/seed/u3/200", RelationshipType = "parent", CustomRoleLabel = null }
        };
        return Ok(ApiResponse<List<FamilyMemberDto>>.Ok(family));
    }

    [HttpPost("me/family")]
    public IActionResult AddFamily([FromBody] AddFamilyRequest request)
    {
        return Ok(ApiResponse.Ok(new { message = "Family connection added." }));
    }

    [HttpDelete("me/family/{targetUserId}")]
    public IActionResult RemoveFamily(string targetUserId)
    {
        return Ok(ApiResponse.Ok(new { message = "Family connection removed." }));
    }

    [HttpPost("me/device-token")]
    public IActionResult RegisterDeviceToken([FromBody] DeviceTokenRequest request)
    {
        return Ok(ApiResponse.Ok(new { message = "Device token registered." }));
    }
}

[ApiController]
[Route("v1/schools/{schoolId}/users")]
public sealed class SchoolUsersController : ControllerBase
{
    [HttpGet]
    public IActionResult GetSchoolUsers(string schoolId, [FromQuery] string? role, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var users = new List<UserListItemDto>
        {
            new() { Id = "u2", Name = "Mehmet Kaya", Email = "mehmet@okul-a.com", Role = "Student", Avatar = "https://picsum.photos/seed/u2/200" },
            new() { Id = "u9", Name = "Ali Vural", Email = "ali@okul-a.com", Role = "Student", Avatar = "https://picsum.photos/seed/u9/200" },
            new() { Id = "u1", Name = "Ahmet Yilmaz", Email = "ahmet@okul-a.com", Role = "Teacher/Coach", Avatar = "https://picsum.photos/seed/u1/200" },
            new() { Id = "u7", Name = "Fatma Sahin", Email = "fatma@okul-a.com", Role = "Teacher/Coach", Avatar = "https://picsum.photos/seed/u7/200" },
            new() { Id = "u8", Name = "Murat Can", Email = "murat@okul-a.com", Role = "Teacher/Coach", Avatar = "https://picsum.photos/seed/u8/200" }
        };

        if (!string.IsNullOrEmpty(role))
            users = users.Where(u => u.Role == role).ToList();

        return Ok(ApiResponse<List<UserListItemDto>>.Ok(users, new MetaData { Page = page, PageSize = pageSize, Total = users.Count }));
    }
}
