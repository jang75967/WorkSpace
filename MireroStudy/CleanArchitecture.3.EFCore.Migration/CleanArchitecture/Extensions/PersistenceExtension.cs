using Infrastructure.EFCore;
using Infrastructure.MongoDB;
using Infrastructure.Persistence.Common.Dapper;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ApplicationDbContext = Infrastructure.EFCore.ApplicationDbContext;

namespace CleanArchitecture.Extensions;

public static class PersistenceExtension
{
    public static IServiceCollection AddEFCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPostgreSql(configuration, Assembly.GetExecutingAssembly().GetName().Name!);
        //services.AddOracle(configuration, Assembly.GetExecutingAssembly().GetName().Name!);
        //services.AddMsSql(configuration, Assembly.GetExecutingAssembly().GetName().Name!);
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
            var url = configuration.GetSection("MsSql").Value ?? configuration.GetConnectionString("MsSql");
            option.UseSqlServer(url,
                b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
        });

        return services;
    }
}
