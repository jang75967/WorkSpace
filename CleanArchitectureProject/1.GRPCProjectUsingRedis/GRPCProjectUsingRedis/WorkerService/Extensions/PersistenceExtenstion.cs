using Application;
using InfraStructrue.Data.Persistence;

namespace WorkerService.Extensions
{
    public static class PersistenceExtenstion
    {
        public static IServiceCollection AddQueue(this IServiceCollection services)
        {
            services.AddSingleton<IQueueService, RedisService>();

            return services;
        }
    }
}
