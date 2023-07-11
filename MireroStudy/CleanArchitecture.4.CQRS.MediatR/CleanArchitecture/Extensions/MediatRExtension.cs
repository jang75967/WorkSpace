using CleanArchitecture.Core.Application.Behaviours;
using MediatR;
using System.Reflection;

namespace CleanArchitecture.Extensions;

public static class MediatRExtension
{
    public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

        return services;
    }
}