using StackExchange.Redis;

namespace RedisLibrary
{
    public interface IConnectionFactory
    {
        public IConfig Configuration { get; set; }
        public IConnectionMultiplexer CreateConnection();
    }
}
