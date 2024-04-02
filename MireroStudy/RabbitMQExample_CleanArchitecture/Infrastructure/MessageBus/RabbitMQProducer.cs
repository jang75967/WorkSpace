using Application.MessageBus;
using Application.Options;
using Domain.Options;
using Domain.Resilience;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;

namespace Infrastructure.MessageBus;

public class RabbitMQProducer : IMessageProducer
{
    private IConnection _connection;
    private IModel _channel;
    private readonly ILogger<RabbitMQProducer> _logger;
    private readonly IOptional<MessageBusOptions> _messageBus;
    private readonly RetryOption _retryOption;

    public RabbitMQProducer(IOptional<MessageBusOptions> options, 
        ILogger<RabbitMQProducer> logger,
        RetryOption retryOption)
    {
        _messageBus = options;
        _logger = logger;
        _retryOption = retryOption ?? throw new ArgumentNullException(nameof(retryOption));

        (_connection, _channel) = Connection(() =>
        {
            var factory = new ConnectionFactory() { Uri = new Uri(_messageBus.Value.Uri) };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            return (connection, channel);
        }, _retryOption);
    }

    private (IConnection, IModel) Connection(Func<(IConnection, IModel)> func, RetryOption retryOption)
    {
        return Policy.Handle<BrokerUnreachableException>()
              .WaitAndRetry(
                  retryOption.RetryCount,
                  delay => TimeSpan.FromMilliseconds(retryOption.RetryDelayMilliseconds),
                  (ex, time) => {
                      _logger.LogError(ex, ex.Message);
                  })
              .Execute(func);
    }

    public void Publish<TCommand>(TCommand command)
    {
        var policy = Policy.Handle<BrokerUnreachableException>()
            .Or<AlreadyClosedException>()
            .Or<SocketException>()
            .Or<AlreadyClosedException>()
            .WaitAndRetry(
               _retryOption.RetryCount,
                delay => TimeSpan.FromMilliseconds(_retryOption.RetryDelayMilliseconds),
                (ex, time) => {
                    _logger.LogError(ex, ex.Message);
                });

        policy.Execute(() => {
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            _channel.QueueDeclare("orders", durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

            var json = JsonConvert.SerializeObject(command);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(exchange: "", routingKey: "orders", body: body);
        });
    }
}
