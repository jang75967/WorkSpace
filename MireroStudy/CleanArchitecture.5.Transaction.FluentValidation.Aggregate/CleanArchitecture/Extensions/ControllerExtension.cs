using CleanArchitecture.Controllers;
using CleanArchitecture.Services;

namespace CleanArchitecture.Extensions;

public static class ControllerExtension
{
    public static IEndpointRouteBuilder AddControllers(this IEndpointRouteBuilder app)
    {
        app.MapGrpcService<ActivityController>();
        app.MapGrpcService<UserController>();
        app.MapGrpcService<GroupController>();
        return app;
    }
}
