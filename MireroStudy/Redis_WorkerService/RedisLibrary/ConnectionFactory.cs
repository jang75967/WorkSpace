using StackExchange.Redis;

namespace RedisLibrary
{
    public interface IConnectionFactory
    {
        public IConfiguration Configuration { get; set; }
        public IConnectionMultiplexer CreateConnection();
    }

    public class ConnectionFactory : IConnectionFactory
    {
        public IConfiguration Configuration { get; set; }

        public ConnectionFactory(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConnectionMultiplexer CreateConnection()
        {
            return ConnectionMultiplexer.Connect(Configuration.GetAddress());
        }
    }
}
