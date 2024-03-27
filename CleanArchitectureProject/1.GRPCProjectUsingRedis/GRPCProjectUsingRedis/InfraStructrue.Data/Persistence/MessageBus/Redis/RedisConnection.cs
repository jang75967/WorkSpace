using Domain.MessageBus.Configuration;
using Domain.MessageBus.Connection;
using Polly;
using StackExchange.Redis;

namespace InfraStructrue.Data.Persistence.MessageBus.Redis
{
    internal class RedisConnection : IConnection, IDisposable
    {
        private IConfiguration _configuration;
        public IConnectionMultiplexer Connection = default!;

        public bool IsConnected => Connection != null && Connection.IsConnected;

        public RedisConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void CreateConnection()
        {
            // retry 정책 설정 (RedisConnectionException 또는 다른 종류의 Exception 이 발생했을 때 재시도 수행)
            var retryPolicy = Policy
                .Handle<RedisConnectionException>()
                .Or<Exception>()
                .WaitAndRetry(retryCount: 3, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(retryAttempt));

            // retry 정책 실행
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
