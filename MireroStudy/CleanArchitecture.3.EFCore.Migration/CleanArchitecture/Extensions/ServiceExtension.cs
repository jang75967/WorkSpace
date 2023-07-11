using CleanArchitecture.Core.Application;
using CleanArchitecture.Services;

namespace CleanArchitecture.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IGroupService, GroupService>();

        return services;
    }
}
