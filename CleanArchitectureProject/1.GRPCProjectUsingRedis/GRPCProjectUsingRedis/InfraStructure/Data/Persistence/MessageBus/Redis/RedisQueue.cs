using Application.Persistences;
using Domain.MessageBus.Configuration;
using Domain.Resilience;
using StackExchange.Redis;

namespace InfraStructrue.Data.Persistence.MessageBus.Redis
{
    public class RedisQueue : IQueue
    {
        private readonly RedisConnection _connection;
        private readonly string _queueName;
        private ITransaction _transaction = default!;

        public RedisQueue(IConfiguration configuration, RetryOption retryOption)
        {
            _connection = new RedisConnection(configuration, retryOption);
            _connection.CreateConnection();
            _queueName = configuration.GetQueueName();
        }

        #region Synchronous

        public void BeginTransaction(CancellationToken cancellationToken = default)
        {
            _transaction = _connection.Connection.GetDatabase().CreateTransaction();
        }

        public bool Execute(CancellationToken cancellationToken = default)
        {
            return _transaction.Execute();
        }

        public long Enqueue(string value, CancellationToken cancellationToken = default)
        {
            return _connection.Connection.GetDatabase().ListLeftPush(_queueName, value);
        }

        public string Dequeue(CancellationToken cancellationToken = default)
        {
            var result = _connection.Connection.GetDatabase().ListRightPop(_queueName);

            if (result.IsNull)
                throw new InvalidOperationException("Queue is empty.");

            return result.ToString();
        }

        public long GetQueueLength(CancellationToken cancellationToken = default)
        {
            return _connection.Connection.GetDatabase().ListLength(_queueName);
        }

        public IList<string> GetAllItems(CancellationToken cancellationToken = default)
        {
            var redisValues = _connection.Connection.GetDatabase().ListRange(_queueName, 0, -1);
            return redisValues.Select(x => x.ToString()).ToList();
        }

        public int Remove(string value, long count = 1, CancellationToken cancellationToken = default)
        {
            var removeItems = _connection.Connection.GetDatabase().ListRemove(_queueName, value.ToString(), count);
            return Convert.ToInt32(removeItems);
        }

        #endregion

        #region Asynchronous

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = _connection.Connection.GetDatabase().CreateTransaction();
            await Task.CompletedTask;
        }

        public async Task<bool> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return await _transaction.ExecuteAsync();
        }

        public async Task<long> EnqueueAsync(string value, CancellationToken cancellationToken = default)
        {
            return await _connection.Connection.GetDatabase().ListLeftPushAsync(_queueName, value);
        }

        public async Task<string> DequeueAsync(CancellationToken cancellationToken = default)
        {
            var result = await _connection.Connection.GetDatabase().ListRightPopAsync(_queueName);

            if (result.IsNull)
                throw new InvalidOperationException("Queue is empty.");

            return result.ToString();
        }

        public async Task<long> GetQueueLengthAsync(CancellationToken cancellationToken = default)
        {
            return await _connection.Connection.GetDatabase().ListLengthAsync(_queueName);
        }

        public async Task<IList<string>> GetAllItemsAsync(CancellationToken cancellationToken = default)
        {
            var redisValues = await _connection.Connection.GetDatabase().ListRangeAsync(_queueName, 0, -1);
            var result = redisValues.Select(x => x.ToString()).ToList();
            return await Task.FromResult(result);
        }

        public async Task<int> RemoveAsync(string value, long count = 1, CancellationToken cancellationToken = default)
        {
            var removeItems = _connection.Connection.GetDatabase().ListRemoveAsync(_queueName, value.ToString(), count);
            return await Task.FromResult(Convert.ToInt32(removeItems));
        }

        #endregion

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
        ~RedisQueue()
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
