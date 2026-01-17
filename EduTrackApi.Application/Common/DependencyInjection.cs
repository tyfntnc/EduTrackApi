using EduTrackApi.Application.Common.Interfaces;
using EduTrackApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EduTrackApi.Application.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<EduTrackDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            //options.UseSqlServer(connectionString);
        });

        // Fully qualify the interface to avoid ambiguity
        services.AddScoped(sp => (IEduTrackDbContext)sp.GetRequiredService<EduTrackDbContext>());

        return services;
    }
}