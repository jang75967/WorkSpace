namespace Application.MessageBus;

public interface IMessageProducer
{
    void Publish<TCommand>(TCommand command);
}
