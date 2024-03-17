using StackExchange.Redis;

namespace RedisLibrary
{
    public interface IConnectionFactory
    {
        public IConfiguration Configuration { get; set; }
        public IConnectionMultiplexer CreateConnection();
    }
}
