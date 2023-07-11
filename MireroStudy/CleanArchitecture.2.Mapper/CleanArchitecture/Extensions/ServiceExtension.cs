using Application;
using CleanArchitecture.Services;

namespace CleanArchitecture.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IUserService, UserService>();
        return services;
    }
}
