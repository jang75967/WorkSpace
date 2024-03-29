using Domain.MessageBus;
using Domain.MessageBus.Address;
using Domain.MessageBus.Configuration;
using Microsoft.Extensions.Options;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using IMessageBusConfiguration = Domain.MessageBus.Configuration.IConfiguration;

namespace WorkerService.Extensions
{
    public static class MessageBusOptionExtension
    {
        public static IServiceCollection AddMessageBoxOptionConfigure(this IServiceCollection services)
        {
            services.AddOptions<MessageBusOption>()
                .Configure<IConfiguration>((options, configuration) =>
                {
                    configuration.GetSection("MessageBus").Bind(options);
                });

            return services;
        }

        public static IServiceCollection AddMessageBusOptionSetup(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBusConfiguration>(provider =>
            {
                var configuration = provider.GetService<IConfiguration>()!;
                var options = provider.GetRequiredService<IOptions<MessageBusOption>>().Value;
                var address = new Address(options.HostName, options.Port);
                return new Configuration(address, "test-queue");
            });

            return services;
        }
    }
}
