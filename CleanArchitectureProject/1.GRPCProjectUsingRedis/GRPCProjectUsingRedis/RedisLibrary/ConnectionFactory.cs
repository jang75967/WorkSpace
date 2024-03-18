using Polly;
using StackExchange.Redis;

namespace RedisLibrary
{
    public class ConnectionFactory : IConnectionFactory
    {
        public IConfig Configuration { get; set; }

        public ConnectionFactory(IConfig configuration)
        {
            Configuration = configuration;
        }

        public IConnectionMultiplexer CreateConnection()
        {
            //return ConnectionMultiplexer.Connect(Configuration.GetAddress());

            // retry 정책 설정 (RedisConnectionException 또는 다른 종류의 Exception 이 발생했을 때 재시도 수행)
            var retryPolicy = Policy
                .Handle<RedisConnectionException>()
                .Or<Exception>()
                .WaitAndRetry(retryCount: 3, sleepDurationProvider: retryAttempt =>  TimeSpan.FromSeconds(retryAttempt));


            // retry 정책 실행
            return retryPolicy.Execute(() =>
            {
                return ConnectionMultiplexer.Connect(Configuration.GetAddress());
            });
        }
    }
}
