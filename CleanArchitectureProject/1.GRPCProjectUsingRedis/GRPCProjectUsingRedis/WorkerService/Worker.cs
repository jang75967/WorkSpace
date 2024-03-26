using Application;
using InfraStructrue.Data.Persistence.MessageBus;

namespace GRPCProejctUsingRedis
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IQueue _queue;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _queue = new RedisManager();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var result = _queue.DequeueAsync(stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(result.ToString());
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
