using Application.Options;
using Application.RabbitMQ;
using Domain.Options;
using Domain.Resilience;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Text;

namespace SubScriber.RabbitMQ;

public class RabbitMQSubscriber : IMessageSubscriber
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IOptional<MessageBusOptions> _messageBus;
    private readonly ILogger<RabbitMQSubscriber> _logger;
    private readonly RetryOption _retryOption;

    public RabbitMQSubscriber(IOptional<MessageBusOptions> options,
        ILogger<RabbitMQSubscriber> logger,
        RetryOption retryOption)
    {
        _messageBus = options;
        _logger = logger;
        _retryOption = retryOption ?? throw new ArgumentNullException(nameof(retryOption));

        (_connection, _channel) = Connection(() =>
        {
            logger.LogInformation($"Rabbit Connection");
            var factory = new ConnectionFactory() { Uri = new Uri(_messageBus.Value.Uri), DispatchConsumersAsync = true };
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
                  (ex, time) =>
                  {
                      _logger.LogError(ex, ex.Message);
                  })
              .Execute(func);
    }

    public void Subscribe<TCommandHandler>(TCommandHandler commandHandler)
    {
        _channel.QueueDeclare("orders", durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (model, eventArgs) =>
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Message received: {message}");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                try
                {
                    // 메시지 처리 후 수동으로 Acknowledgement 보내기
                    _channel.BasicAck(eventArgs.DeliveryTag, multiple: false);
                }
                catch
                {
                }
            }
        };

        _channel.BasicConsume(queue: "orders", autoAck: false, consumer: consumer);
    }
}
