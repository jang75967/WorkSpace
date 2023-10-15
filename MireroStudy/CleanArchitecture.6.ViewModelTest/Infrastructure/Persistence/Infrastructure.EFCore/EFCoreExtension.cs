using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure.EFCore;

public static class EFCoreExtension
{
    public static IServiceCollection AddPostgreSql(this IServiceCollection services, IConfiguration configuration, string assemblyName)
    {
        services.AddDbContextFactory<ApplicationDbContext>(option =>
        {
            var url = configuration.GetSection("PostgreSql").Value ?? configuration.GetConnectionString("PostgreSql");
            option.UseNpgsql(url,
                b => b.MigrationsAssembly(assemblyName));
        });
        return services;
    }

    public static IServiceCollection AddInMemory(this IServiceCollection services, string assemblyName)
    {
        services.AddDbContext<ApplicationDbContext>(option =>
        {
           option.UseInMemoryDatabase(assemblyName);
        });
        return services;
    }
}
