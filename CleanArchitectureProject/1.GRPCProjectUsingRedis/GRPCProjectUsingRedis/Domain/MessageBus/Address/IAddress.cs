namespace Domain.MessageBus.Address
{
    public interface IAddress
    {
        string IP { get; set; }
        string Port { get; set; }

        string Get();
    }
}
