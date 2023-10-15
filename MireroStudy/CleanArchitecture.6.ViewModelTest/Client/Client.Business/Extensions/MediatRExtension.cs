
using Client.Business.Core.Application.Behaviors;
using LanguageExt;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Client.Business.Extensions;

public static class MediatRExtension
{
    public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RetryPolicyBehavior<,>));
       

        return services;
    }
}