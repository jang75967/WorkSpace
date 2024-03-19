using RedisLibrary;

namespace GRPCProejctUsingRedis
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IQueue _queue;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

            IAddress address = new Address("127.0.0.1", "6379");
            IConfig configuration = new Configuration(address, "test-queue");
            IConnectionFactory connectionFactory = new ConnectionFactory(configuration);
            _queue = new Queue(connectionFactory);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = _queue.Dequeue();
                _logger.LogInformation(result);
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
