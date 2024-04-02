namespace Application.RabbitMQ;

public interface IMessageSubscriber
{
    void Subscribe<TCommandHandler>(TCommandHandler commandHandler);
}
