namespace Domain.MessageBus.Connection
{
    public interface IConnection
    {
        bool IsConnected { get; }
        void CreateConnection();
        void CloseConnection();
    }
}
