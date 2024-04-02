using Application.RabbitMQ;
using SubScriber.Extensions;
using SubScriber.RabbitMQ;

namespace SubScriber
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", false, true)
                        .AddEnvironmentVariables()
                        .Build();

                    services.AddSingleton<IMessageSubscriber, RabbitMQSubscriber>();
                    services.AddHostedService<Worker>();
                    services.AddOptionExtension(configuration);
                    services.AddResilience(configuration);

                })
                .Build();

            host.Run();
        }
    }
}