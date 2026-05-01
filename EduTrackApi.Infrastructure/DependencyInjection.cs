using EduTrackApi.Application.Common.Interfaces;
using EduTrackApi.Infrastructure.Persistence;
using EduTrackApi.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace EduTrackApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Connection string: check for encrypted version first, fall back to plain
        var connectionString = configuration["ConnectionStrings:DefaultConnectionEncrypted"];
        var encryptionKey = configuration["ConnectionStrings:EncryptionKey"];

        if (!string.IsNullOrEmpty(connectionString) && !string.IsNullOrEmpty(encryptionKey))
        {
            connectionString = ConnectionStringProtector.Decrypt(connectionString, encryptionKey);
        }
        else
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        services.AddDbContext<EduTrackDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.CommandTimeout(60);
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorCodesToAdd: null);
            });
        });

        services.AddScoped<IEduTrackDbContext>(sp => sp.GetRequiredService<EduTrackDbContext>());
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}