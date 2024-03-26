using Application;
using InfraStructrue.Data.Persistence.MessageBus;

namespace WorkerService.Extensions
{
    public static class PersistenceExtenstion
    {
        public static IServiceCollection AddQueue(this IServiceCollection services)
        {
            services.AddSingleton<IQueue, RedisManager>();

            return services;
        }
    }
}
