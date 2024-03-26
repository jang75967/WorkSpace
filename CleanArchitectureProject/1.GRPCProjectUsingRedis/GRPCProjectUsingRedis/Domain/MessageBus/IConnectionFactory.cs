using StackExchange.Redis;

namespace Domain
{
    public interface IConnectionFactory
    {
        public IConfig Configuration { get; set; }
        public IConnectionMultiplexer CreateConnection();
    }
}
