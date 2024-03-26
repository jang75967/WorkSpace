using Application;
using Domain;
using Domain.MessageBus;
using StackExchange.Redis;

namespace InfraStructrue.Data.Persistence.MessageBus
{
    public class RedisManager : IQueue
    {
        private IConnectionFactory _connectionFactory;
        private IConnectionMultiplexer _connection;
        private ITransaction _transaction = default!;
        private string _queueName = default!;

        public RedisManager()
        {
            //IAddress address = new Address("127.0.0.1", "6379");
            IAddress address = new Address("192.168.100.142", "6379");
            IConfig configuration = new Configuration(address, "test-queue");
            IConnectionFactory connectionFactory = new ConnectionFactory(configuration);

            _connectionFactory = connectionFactory;
            _connection = _connectionFactory.CreateConnection();
            _queueName = _connectionFactory.Configuration.GetQueueName();
        }

        public async Task BeginTranscationAsync(CancellationToken cancellationToken = default)
        {
            _transaction = _connection.GetDatabase().CreateTransaction();
            await Task.CompletedTask;
        }

        public async Task<bool> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return await _transaction.ExecuteAsync();
        }

        public async Task<long> EnqueueAsync(string value, CancellationToken cancellationToken = default)
        {
            return await _connection.GetDatabase().ListLeftPushAsync(_queueName, value);
        }

        public async Task<string> DequeueAsync(CancellationToken cancellationToken = default)
        {
            return await _connection.GetDatabase().ListRightPopAsync(_queueName);
        }

        public async Task<long> GetQueueLengthAsync(CancellationToken cancellationToken = default)
        {
            return await _connection.GetDatabase().ListLengthAsync(_queueName);
        }

        public async Task<IList<string>> GetAllItemsAsync(CancellationToken cancellationToken = default)
        {
            var redisValues = await _connection.GetDatabase().ListRangeAsync(_queueName, 0, -1);
            var result = redisValues.Select(x => x.ToString()).ToList();
            return await Task.FromResult(result);
        }

        public async Task<int> RemoveAsync(string value, long count = 1, CancellationToken cancellationToken = default)
        {
            var removeItems = _connection.GetDatabase().ListRemoveAsync(_queueName, value.ToString(), count);
            return await Task.FromResult(Convert.ToInt32(removeItems));
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing, CancellationToken cancellationToken = default)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                _connection.Dispose();

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~RedisManager()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
