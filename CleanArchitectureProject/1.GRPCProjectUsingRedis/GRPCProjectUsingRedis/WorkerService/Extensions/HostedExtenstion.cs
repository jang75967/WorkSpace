using GRPCProejctUsingRedis;

namespace WorkerService.Extensions
{
    public static class HostedExtenstion
    {
        public static IServiceCollection AddBackgroundService(this IServiceCollection services)
        {
            services.AddHostedService<Worker>();

            return services;
        }
    }
}
