using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure.EFCore;

public static class EFCoreExtension
{
    public static IServiceCollection AddPostgreSql(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<ApplicationDbContext>(option =>
        {
            var url = configuration.GetSection("PostgreSql").Value ?? configuration.GetConnectionString("PostgreSql");
            option.UseNpgsql(url,
                b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
        });
        return services;
    }

    public static IServiceCollection AddMsSql(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<ApplicationDbContext>(option =>
        {
            var url = configuration.GetSection("MsSql").Value ?? configuration.GetConnectionString("MsSql");
            option.UseSqlServer(url,
                b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
        });
        return services;
    }

    public static IServiceCollection AddOracle(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<ApplicationDbContext>(option =>
        {
            var url = configuration.GetSection("Oracle").Value ?? configuration.GetConnectionString("Oracle");
            option.UseOracle(url,
                b =>
                {
                    b.UseOracleSQLCompatibility("11");
                    b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                });
        });
        return services;
    }
}
