using Application.RabbitMQ;

namespace SubScriber
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMessageSubscriber _messageSubscriber;

        public Worker(ILogger<Worker> logger, IMessageSubscriber messageSubscriber)
        {
            _logger = logger;
            _messageSubscriber = messageSubscriber;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                _messageSubscriber.Subscribe(this);

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
