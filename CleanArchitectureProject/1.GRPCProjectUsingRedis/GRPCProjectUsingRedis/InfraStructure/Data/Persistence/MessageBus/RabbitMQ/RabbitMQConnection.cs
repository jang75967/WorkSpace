using Domain.MessageBus.Configuration;
using Domain.Resilience;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using IMessageBusConnection = Domain.MessageBus.Connection.IConnection;
using IRabbitMQConnection = RabbitMQ.Client.IConnection;

namespace InfraStructure.Data.Persistence.MessageBus.RabbitMQ
{
    internal class RabbitMQConnection : IMessageBusConnection, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly RetryOption _retryOption;
        public IRabbitMQConnection Connection = default!;

        public bool IsConnected => Connection != null && Connection.IsOpen;

        public RabbitMQConnection(IConfiguration configuration, RetryOption retryOption)
        {
            _configuration = configuration;
            _retryOption = retryOption;
        }

        public void CreateConnection()
        {
            var retryPolicy = Policy
                .Handle<RabbitMQClientException>()
                .Or<Exception>()
                .WaitAndRetry(retryCount: _retryOption.RetryCount, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(_retryOption.RetryDelayMilliseconds));

            Connection = retryPolicy.Execute(() =>
            {
                var connectionFactory = new ConnectionFactory()
                {
                    HostName = _configuration.Address.IP,
                    Port = Convert.ToInt32(_configuration.Address.Port),
                    //UserName = "jdg",
                    //Password = "7596"
                };

                return connectionFactory.CreateConnection();
            });
        }

        public void CloseConnection()
        {
            Connection?.Close();
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }
    }
}
