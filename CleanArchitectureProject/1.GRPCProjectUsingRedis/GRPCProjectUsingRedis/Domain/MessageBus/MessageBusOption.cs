namespace Domain.MessageBus
{
    public class MessageBusOptions
    {
        public Dictionary<string, MessageBusOption> Options { get; set; } = new Dictionary<string, MessageBusOption>();
    }
    
    public record MessageBusOption
    {
        public string HostName { get; set; } = default!;
        public string Port { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string QueueName { get; set; } = default!;
    }
}
