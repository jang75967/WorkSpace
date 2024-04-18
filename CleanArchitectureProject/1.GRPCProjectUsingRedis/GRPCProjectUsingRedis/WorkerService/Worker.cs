using Application.Persistences;

namespace GRPCProejctUsingRedis
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IQueue _queue;

        public Worker(ILogger<Worker> logger, IQueue queue)
        {
            _logger = logger;
            _queue = queue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //_queue.Dequeue();
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
