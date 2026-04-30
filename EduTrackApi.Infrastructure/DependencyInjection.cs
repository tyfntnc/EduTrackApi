using EduTrackApi.Application.Common.Interfaces;
using EduTrackApi.Infrastructure.Persistence;
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
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<EduTrackDbContext>(options =>
        {
            options.UseNpgsql(dataSource);
        });

        services.AddScoped<IEduTrackDbContext>(sp => sp.GetRequiredService<EduTrackDbContext>());
        
        return services;
    }
}