using Domain.MessageBus;
using Domain.MessageBus.Address;
using Domain.MessageBus.Configuration;
using Domain.Resilience;
using Microsoft.Extensions.Options;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using IMessageBusConfiguration = Domain.MessageBus.Configuration.IConfiguration;

namespace WorkerService.Extensions
{
    public static class MessageBusOptionExtension
    {
        public static IServiceCollection AddMessageBusOption(this IServiceCollection services)
        {
            AddMessageBusOptionConfigure(services);
            AddMessageBusOptionSetup(services);

            return services;
        }

        private static IServiceCollection AddMessageBusOptionConfigure(this IServiceCollection services)
        {
            services.AddOptions<MessageBusOption>()
                .Configure<IConfiguration>((options, configuration) =>
                {
                    configuration.GetSection("MessageBus").Bind(options);
                });
            services.AddOptions<RetryOption>()
                .Configure<IConfiguration>((options, configuration) =>
                {
                    configuration.GetSection("RetryOption").Bind(options);
                });

            return services;
        }

        private static IServiceCollection AddMessageBusOptionSetup(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBusConfiguration>(provider =>
            {
                var configuration = provider.GetService<IConfiguration>()!;
                var options = provider.GetRequiredService<IOptions<MessageBusOption>>().Value;

                var address = new Address(options.HostName, options.Port);
                return new Configuration(address, options.QueueName);
            });
            services.AddSingleton(provider =>
            {
                return provider.GetRequiredService<IOptions<RetryOption>>().Value;
            });

            return services;
        }
    }
}
