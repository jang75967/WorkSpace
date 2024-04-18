using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace InfraStructure.Data.Persistence.EFCore
{
    public static class EFCoreExtension
    {
        public static IServiceCollection AddPostgresSql(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetSection("Postgres").Value ?? configuration?.GetConnectionString("Postgres");

            services.AddDbContextFactory<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString, builder => builder.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name))
                // 디버깅 모드에서 사용
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
            });

            return services;
        }
    }
}
