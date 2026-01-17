using EduTrackApi.Application.Common.Interfaces;
using EduTrackApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EduTrackApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<EduTrackDbContext>(options =>
        {
            //options.UseNpgsql(connectionString);
            //options.UseSqlServer(connectionString);
            
        });

        services.AddScoped<IEduTrackDbContext>(sp => sp.GetRequiredService<EduTrackDbContext>());

        return services;
    }
}