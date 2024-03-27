using Domain.MessageBus.Address;

namespace Domain.MessageBus.Configuration
{
    public interface IConfiguration
    {
        IAddress Address { get; set; }
        string QueueName { get; set; }

        public string GetQueueName();
        public string GetAddress();
    }
}
