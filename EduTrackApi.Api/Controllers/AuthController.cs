using EduTrackApi.Application.Auth.Models;
using EduTrackApi.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackApi.Api.Controllers;

[ApiController]
[Route("v1/auth")]
public sealed class AuthController : ControllerBase
{
    private static readonly AuthUserDto MockUser = new()
    {
        Id = "u1",
        Name = "Ahmet Yilmaz",
        Email = "ahmet@okul-a.com",
        Role = "Teacher/Coach",
        SchoolId = "school-a",
        Avatar = "https://picsum.photos/seed/u1/200"
    };

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request.Email == "ahmet@okul-a.com" && request.Password == "Sifre123")
        {
            return Ok(ApiResponse<AuthResponse>.Ok(new AuthResponse
            {
                AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.mock",
                RefreshToken = "mock-refresh-token",
                ExpiresIn = 3600,
                User = MockUser
            }));
        }
        return Unauthorized(ApiResponse<AuthResponse>.Fail("INVALID_CREDENTIALS", "Email or password is incorrect."));
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        return StatusCode(201, ApiResponse<AuthResponse>.Ok(new AuthResponse
        {
            AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.mock",
            RefreshToken = "mock-refresh-token",
            ExpiresIn = 3600,
            User = new AuthUserDto
            {
                Id = "u-new-123",
                Name = request.Name,
                Email = request.Email,
                Role = "Student",
                SchoolId = null,
                Avatar = null
            }
        }));
    }

    [HttpPost("forgot-password")]
    public IActionResult ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        return Ok(ApiResponse.Ok(new { message = "Password reset link has been sent.", maskedEmail = "ah***@okul-a.com" }));
    }

    [HttpPost("reset-password")]
    public IActionResult ResetPassword([FromBody] ResetPasswordRequest request)
    {
        return Ok(ApiResponse.Ok(new { message = "Password updated successfully." }));
    }

    [HttpPost("refresh-token")]
    public IActionResult RefreshToken([FromBody] RefreshTokenRequest request)
    {
        return Ok(ApiResponse.Ok(new { accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.refreshed", expiresIn = 3600 }));
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok(ApiResponse.Ok(new { message = "Session closed." }));
    }

    [HttpPost("google")]
    public IActionResult GoogleLogin([FromBody] SocialLoginRequest request)
    {
        return Ok(ApiResponse<AuthResponse>.Ok(new AuthResponse
        {
            AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.google",
            RefreshToken = "mock-refresh-token-google",
            ExpiresIn = 3600,
            User = MockUser
        }));
    }

    [HttpPost("facebook")]
    public IActionResult FacebookLogin([FromBody] SocialLoginRequest request)
    {
        return Ok(ApiResponse<AuthResponse>.Ok(new AuthResponse
        {
            AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.facebook",
            RefreshToken = "mock-refresh-token-facebook",
            ExpiresIn = 3600,
            User = MockUser
        }));
    }
}
