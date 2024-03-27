using GRPCProejctUsingRedis;

namespace WorkerService.Extensions
{
    public static class HostedExtenstion
    {
        public static IServiceCollection AddWorkerService(this IServiceCollection services)
        {
            services.AddHostedService<Worker>();

            return services;
        }
    }
}
