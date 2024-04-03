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
            services.AddOptions<MessageBusOptions>()
                .Configure<IConfiguration>((options, configuration) =>
                {
                    var messageBusSections = configuration.GetSection("MessageBus").GetChildren();

                    foreach (var messageBusSection in messageBusSections)
                    {
                        var messageBusOption = new MessageBusOption();
                        messageBusSection.Bind(messageBusOption);
                        options.Options.Add(messageBusSection.Key, messageBusOption);
                    }
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
                var options = provider.GetRequiredService<IOptions<MessageBusOptions>>().Value;

                //var option = options.Options["redis"];
                var option = options.Options["rabbitmq"];

                var address = new Address(option.HostName, option.Port, option.UserName, option.Password);
                return new Configuration(address, option.QueueName);
            });
            services.AddSingleton(provider =>
            {
                return provider.GetRequiredService<IOptions<RetryOption>>().Value;
            });

            return services;
        }
    }
}
