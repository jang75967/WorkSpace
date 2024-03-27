using Application;
using Domain.MessageBus.Address;
using Domain.MessageBus.Configuration;
using InfraStructrue.Data.Persistence.MessageBus.Redis;
using IConfiguration = Domain.MessageBus.Configuration.IConfiguration;

namespace WorkerService.Extensions
{
    public static class PersistenceExtension
    {
        public static IServiceCollection AddQueue(this IServiceCollection services)
        {
            services.AddSingleton(GetConfiguration());
            services.AddSingleton<IQueue, RedisQueue>();

            return services;
        }

        private static IConfiguration GetConfiguration()
        {
            //IAddress address = new Address("127.0.0.1", "6379");
            IAddress address = new Address("192.168.100.142", "6379");
            return new Configuration(address, "test-queue");
        }
    }
}
