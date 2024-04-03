using Domain.MessageBus.Configuration;
using Domain.MessageBus.Connection;
using Domain.Resilience;
using Polly;
using StackExchange.Redis;

namespace InfraStructrue.Data.Persistence.MessageBus.Redis
{
    internal class RedisConnection : IConnection, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly RetryOption _retryOption;
        public IConnectionMultiplexer Connection = default!;

        public bool IsConnected => Connection != null && Connection.IsConnected;

        public RedisConnection(IConfiguration configuration, RetryOption retryOption)
        {
            _configuration = configuration;
            _retryOption = retryOption;
        }

        public void CreateConnection()
        {
            var retryPolicy = Policy
                .Handle<RedisConnectionException>()
                .Or<Exception>()
                .WaitAndRetry(retryCount: _retryOption.RetryCount, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(_retryOption.RetryDelayMilliseconds));

            Connection = retryPolicy.Execute(() =>
            {
                return ConnectionMultiplexer.Connect(_configuration.GetAddress());
            });
        }

        public void CloseConnection()
        {
            Connection?.Close();
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }
    }
}
