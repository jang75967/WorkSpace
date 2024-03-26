using Domain.MessageBus.Configuration;
using StackExchange.Redis;

namespace Domain.MessageBus.Connection
{
    public interface IConnectionFactory
    {
        public IConfig Configuration { get; set; }
        public IConnectionMultiplexer CreateConnection();
    }
}
