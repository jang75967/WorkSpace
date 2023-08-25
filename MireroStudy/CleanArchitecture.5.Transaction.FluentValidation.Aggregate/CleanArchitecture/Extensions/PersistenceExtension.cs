using Infrastructure.EFCore;
using System.Reflection;

namespace CleanArchitecture.Extensions;

public static class PersistenceExtension
{
    public static IServiceCollection AddEFCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPostgreSql(configuration, Assembly.GetExecutingAssembly().GetName().Name!);
        return services;
    }
}
