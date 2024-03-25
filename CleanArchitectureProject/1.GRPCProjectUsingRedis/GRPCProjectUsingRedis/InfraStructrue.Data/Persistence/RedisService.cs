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

        public async Task Push(string input)
        {
            _queue.Enqueue(input);
            await Task.FromResult(0);
        }

        public async Task Pop()
        {
            _queue.Dequeue();
            await Task.FromResult(0);
        }
    }
}
