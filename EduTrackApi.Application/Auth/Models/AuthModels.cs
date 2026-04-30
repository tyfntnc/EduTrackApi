namespace EduTrackApi.Application.Auth.Models;

public sealed class LoginRequest
{
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
    public bool RememberMe { get; init; }
}

public sealed class RegisterRequest
{
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Phone { get; init; } = default!;
    public string Password { get; init; } = default!;
    public bool KvkkConsent { get; init; }
}

public sealed class ForgotPasswordRequest
{
    public string Email { get; init; } = default!;
}

public sealed class ResetPasswordRequest
{
    public string Token { get; init; } = default!;
    public string NewPassword { get; init; } = default!;
}

public sealed class RefreshTokenRequest
{
    public string RefreshToken { get; init; } = default!;
}

public sealed class SocialLoginRequest
{
    public string? IdToken { get; init; }
    public string? AccessToken { get; init; }
}

public sealed class AuthResponse
{
    public string AccessToken { get; init; } = default!;
    public string RefreshToken { get; init; } = default!;
    public int ExpiresIn { get; init; }
    public AuthUserDto User { get; init; } = default!;
}

public sealed class AuthUserDto
{
    public string Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Role { get; init; } = default!;
    public string? SchoolId { get; init; }
    public string? Avatar { get; init; }
}
