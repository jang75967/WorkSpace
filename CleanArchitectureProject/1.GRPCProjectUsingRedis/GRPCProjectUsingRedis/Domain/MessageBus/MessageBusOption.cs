namespace Domain.MessageBus
{
    public record MessageBusOption
    {
        public string HostName { get; set; } = default!;
        public string Port { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
