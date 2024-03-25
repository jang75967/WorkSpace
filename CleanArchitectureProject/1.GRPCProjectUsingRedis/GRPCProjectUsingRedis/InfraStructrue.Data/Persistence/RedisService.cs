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

        public async Task BeginTranscationAsync()
        {
            _queue.BeginTranscation();
            await Task.CompletedTask;
        }

        public async Task<bool> ExecuteAsync()
        {
            bool result = _queue.Execute();
            return await Task.FromResult(result);
        }

        public async Task PushAsync(string input)
        {
            _queue.Enqueue(input);
            await Task.CompletedTask;
        }

        public async Task PopAsync()
        {
            _queue.Dequeue();
            await Task.CompletedTask;
        }
    }
}
