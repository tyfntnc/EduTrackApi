using EduTrackApi.Application.Auth.Models;
using EduTrackApi.Application.Common.Interfaces;
using EduTrackApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduTrackApi.Infrastructure.Security;

public sealed class AuthService : IAuthService
{
    private readonly IEduTrackDbContext _db;
    private readonly IJwtTokenService _jwt;

    public AuthService(IEduTrackDbContext db, IJwtTokenService jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    public async Task<AuthResponse?> LoginAsync(string email, string password)
    {
        var user = await _db.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user is null || string.IsNullOrEmpty(user.PasswordHash))
            return null;

        // TODO: Proje stabilize olunca BCrypt.Verify'e geri dön
        if (password != user.PasswordHash)
            return null;

        return await GenerateAuthResponseAsync(user);
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        var exists = await _db.Users.AnyAsync(u => u.Email == request.Email);
        if (exists)
            return null;

        var user = new User
        {
            Id = Guid.NewGuid().ToString("N")[..12],
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.Phone,
            // TODO: Proje stabilize olunca BCrypt.HashPassword'e geri dön
            PasswordHash = request.Password,
            RoleId = 5 // Default: Student
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        // Reload with Role navigation
        user = await _db.Users.Include(u => u.Role).FirstAsync(u => u.Id == user.Id);
        return await GenerateAuthResponseAsync(user);
    }

    public async Task<AuthResponse?> RefreshTokenAsync(string refreshToken)
    {
        var user = await _db.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiry > DateTime.UtcNow);

        if (user is null)
            return null;

        return await GenerateAuthResponseAsync(user);
    }

    public async Task RevokeRefreshTokenAsync(string userId)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user is not null)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await _db.SaveChangesAsync();
        }
    }

    private async Task<AuthResponse> GenerateAuthResponseAsync(User user)
    {
        var accessToken = _jwt.GenerateAccessToken(user);
        var refreshToken = _jwt.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _db.SaveChangesAsync();

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = 3600,
            User = new AuthUserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role?.Code ?? "STUDENT",
                SchoolId = user.SchoolId,
                Avatar = user.Avatar
            }
        };
    }
}
