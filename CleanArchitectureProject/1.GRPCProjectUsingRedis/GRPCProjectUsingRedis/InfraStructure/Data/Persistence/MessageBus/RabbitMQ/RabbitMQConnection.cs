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
        private IRabbitMQConnection _connection = default!;

        public IModel Model { get; private set; } = default!;

        public bool IsConnected => _connection != null && _connection.IsOpen;

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

            _connection = retryPolicy.Execute(() =>
            {
                var connectionFactory = new ConnectionFactory()
                {
                    HostName = _configuration.Address.IP,
                    Port = Convert.ToInt32(_configuration.Address.Port),
                    UserName = _configuration.Address.UserName,
                    Password = _configuration.Address.Password,
                };

                return connectionFactory.CreateConnection();
            });

            Model = _connection.CreateModel();

            Model.QueueDeclare(
                queue: _configuration.GetQueueName(),
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public void CloseConnection()
        {
            _connection?.Close();
        }

        public void Dispose()
        {
            Model?.Dispose();
            _connection?.Dispose();
        }
    }
}
