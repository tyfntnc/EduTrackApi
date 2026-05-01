using EduTrackApi.Application.Auth.Models;
using EduTrackApi.Application.Common.Interfaces;
using EduTrackApi.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduTrackApi.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request.Email, request.Password);
        if (result is null)
            return Unauthorized(ApiResponse<AuthResponse>.Fail("INVALID_CREDENTIALS", "Email or password is incorrect."));

        return Ok(ApiResponse<AuthResponse>.Ok(result));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        if (result is null)
            return Conflict(ApiResponse<AuthResponse>.Fail("EMAIL_EXISTS", "A user with this email already exists."));

        return StatusCode(201, ApiResponse<AuthResponse>.Ok(result));
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _authService.RefreshTokenAsync(request.RefreshToken);
        if (result is null)
            return Unauthorized(ApiResponse.Fail("INVALID_REFRESH_TOKEN", "Refresh token is invalid or expired."));

        return Ok(ApiResponse<AuthResponse>.Ok(result));
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub");
        if (userId is not null)
            await _authService.RevokeRefreshTokenAsync(userId);

        return Ok(ApiResponse.Ok(new { message = "Session closed." }));
    }

    [HttpPost("forgot-password")]
    public IActionResult ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        // TODO: Implement email sending
        var masked = request.Email.Length > 4
            ? request.Email[..2] + "***" + request.Email[request.Email.IndexOf('@')..]
            : "***";
        return Ok(ApiResponse.Ok(new { message = "Password reset link has been sent.", maskedEmail = masked }));
    }

    [HttpPost("reset-password")]
    public IActionResult ResetPassword([FromBody] ResetPasswordRequest request)
    {
        // TODO: Implement token-based password reset
        return Ok(ApiResponse.Ok(new { message = "Password updated successfully." }));
    }
}


