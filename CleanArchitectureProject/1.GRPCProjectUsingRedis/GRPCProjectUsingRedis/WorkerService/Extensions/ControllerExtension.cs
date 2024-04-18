using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using WorkerService.Controllers;

namespace WorkerService.Extensions
{
    public static class ControllerExtension
    {
        public static IEndpointRouteBuilder AddControllers(this IEndpointRouteBuilder app)
        {
            app.MapGrpcService<UserController>();
            app.MapGrpcService<GroupController>();

            return app;
        }
    }
}
