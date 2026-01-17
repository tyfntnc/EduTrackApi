using EduTrackApi.Application;
using EduTrackApi.Infrastructure;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(); // Swagger servis kaydı

        // Application + Infrastructure
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

        var app = builder.Build();

        // Swagger middleware (UI dahil)
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "EduTrack API V1");
                // options.RoutePrefix = string.Empty; // İstersen root'tan açmak için
            });
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}