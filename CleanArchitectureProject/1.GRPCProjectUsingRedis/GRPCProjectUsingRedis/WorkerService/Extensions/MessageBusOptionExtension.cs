using Domain.MessageBus;
using Domain.MessageBus.Address;
using Domain.MessageBus.Configuration;
using Microsoft.Extensions.Options;
using IMessageBusConfiguration = Domain.MessageBus.Configuration.IConfiguration;

namespace WorkerService.Extensions
{
    public static class MessageBusOptionExtension
    {
        public static IServiceCollection AddMessageBoxOptionConfigure(this IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            return services.Configure<MessageBusOption>(option => configurationRoot.GetSection("MessageBus").Bind(option));
        }

        public static IServiceCollection AddMessageBusOptionSetup(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBusConfiguration>(provider =>
            {
                var options = provider.GetRequiredService<IOptions<MessageBusOption>>().Value;
                var address = new Address(options.HostName, options.Port);
                return new Configuration(address, "test-queue");
            });

            return services;
        }
    }
}
