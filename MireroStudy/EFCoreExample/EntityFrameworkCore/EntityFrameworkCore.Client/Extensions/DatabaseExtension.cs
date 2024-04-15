using EntityFrameworkCore.Data;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Client.Extensions;

public static class DatabaseExtension
{
    public static IServiceCollection AddPostgreSQLDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<FootballLeagueDbContext>();
        services.AddDbContextFactory<FootballLeagueDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("PostgreDb"),
                    b => b.MigrationsAssembly("EntityFrameworkCore.Data"))
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        });

        return services;
    }
}
