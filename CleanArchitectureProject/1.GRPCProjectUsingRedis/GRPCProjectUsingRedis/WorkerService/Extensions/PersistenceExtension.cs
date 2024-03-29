using Application;
using InfraStructrue.Data.Persistence.MessageBus.Redis;

namespace WorkerService.Extensions
{
    public static class PersistenceExtension
    {
        public static IServiceCollection AddQueue(this IServiceCollection services)
        {
            services.AddSingleton<IQueue, RedisQueue>();

            return services;
        }
    }
}
