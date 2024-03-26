using Application;
using RedisLibrary;

namespace InfraStructrue.Data.Persistence
{
    public class RedisService : IQueueService
    {
        private IQueue _queue = default!;

        public RedisService()
        {
            IAddress address = new Address("127.0.0.1", "6379");
            IConfig configuration = new Configuration(address, "test-queue");
            IConnectionFactory connectionFactory = new ConnectionFactory(configuration);
            _queue = new Queue(connectionFactory);
        }

        public async Task BeginTranscationAsync(CancellationToken cancellationToken = default)
        {
            _queue.BeginTranscation(cancellationToken);
            await Task.CompletedTask;
        }

        public async Task<bool> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            bool result = _queue.Execute(cancellationToken);
            return await Task.FromResult(result);
        }

        public async Task PushAsync(string input, CancellationToken cancellationToken = default)
        {
            _queue.Enqueue(input, cancellationToken);
            await Task.CompletedTask;
        }

        public async Task PopAsync(CancellationToken cancellationToken = default)
        {
            _queue.Dequeue(cancellationToken);
            await Task.CompletedTask;
        }
    }
}
