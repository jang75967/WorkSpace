namespace Domain.MessageBus;

public record QueueOptionBase : IQueueOption
{
    public string Name { get; private init; }

    public QueueOptionBase(string name)
    {
        Name = name;
    }
}
