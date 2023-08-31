using Infrastructure.EFCore;
using Infrastructure.MongoDB;
using System.Reflection;

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
}
