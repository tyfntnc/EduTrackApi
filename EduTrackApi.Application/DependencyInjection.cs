using EduTrackApi.Application.Courses.Queries.GetCourses;
using EduTrackApi.Application.Users.Queries.GetUsers;
using Microsoft.Extensions.DependencyInjection;

namespace EduTrackApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Query handlers
        services.AddScoped<GetUsersQueryHandler>();
        services.AddScoped<GetCoursesQueryHandler>();

        return services;
    }
}