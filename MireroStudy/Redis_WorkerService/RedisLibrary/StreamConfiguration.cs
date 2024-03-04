namespace RedisLibrary;

public record StreamConfiguration : IConfiguration
{
    public IAddress Address { get; set; }
    public string QueueName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public StreamConfiguration(IAddress address)
    {
        Address = address;
    }

    public string GetAddress()
    {
        if (Address == null)
            throw new NullReferenceException();
        return Address.Get();
    }

    public string GetQueueName()
    {
        throw new NotImplementedException();
    }
}
