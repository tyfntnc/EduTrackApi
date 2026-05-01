using EduTrackApi.Domain.Entities;

namespace EduTrackApi.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}
