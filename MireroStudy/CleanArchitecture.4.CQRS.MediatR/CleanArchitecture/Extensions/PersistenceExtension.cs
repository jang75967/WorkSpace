using Application.Persistences;
using Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CleanArchitecture.Extensions;

public static class PersistenceExtension
{
    public static IServiceCollection AddEFCore(this IServiceCollection services, IConfiguration configuration)
    {
       // services.AddTransient<IApplicationDbContext, Infrastructure.EFCore.ApplicationDbContext>();
        services.AddPostgreSql(configuration);
        //services.AddOracle(configuration);
        //services.AddMsSql(configuration);
        //services.AddMongoDB(configuration);
        return services;
    }

    // Dapper 추가

    //public static IServiceCollection AddDapper(this IServiceCollection services, IConfiguration configuration)
    //{
    //    return services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
    //}


    public static IServiceCollection AddDapper(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<ApplicationDbContext>(option =>
        {
            var url = configuration.GetSection("PostgreSql").Value ?? configuration.GetConnectionString("PostgreSql");
            option.UseSqlServer(url,
                b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
        });

        return services;
    }
}
