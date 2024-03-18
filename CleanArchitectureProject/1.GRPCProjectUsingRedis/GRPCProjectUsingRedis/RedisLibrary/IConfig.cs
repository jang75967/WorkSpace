namespace RedisLibrary
{
    public interface IConfig
    {
        IAddress Address { get; set; }
        string QueueName { get; set; }

        public string GetQueueName();
        public string GetAddress();
    }
}
