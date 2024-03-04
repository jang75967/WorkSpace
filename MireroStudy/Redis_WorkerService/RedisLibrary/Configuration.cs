namespace RedisLibrary
{
    public record Configuration : IConfiguration
    {
        public IAddress Address { get; set; }

        public string QueueName { get; set; }

        public Configuration(IAddress address, string queueName)

        {
            Address = address;
            QueueName = queueName;
        }

        public string GetQueueName()
        {
            if (QueueName == null)
                throw new NullReferenceException();
            return QueueName;
        }

        public string GetAddress()
        {
            if (Address == null)
                throw new NullReferenceException();
            return Address.Get();
        }
    }
}
