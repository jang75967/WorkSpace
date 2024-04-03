namespace Domain.MessageBus.Address
{
    public interface IAddress
    {
        string IP { get; set; }
        string Port { get; set; }
        string UserName { get; set; }
        string Password { get; set; }

        string Get();
    }
}
