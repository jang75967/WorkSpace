using StackExchange.Redis;

namespace RedisLibrary
{
    public class Queue : IQueue
    {
        private IConnectionFactory _connectionFactory;
        private IConnectionMultiplexer _connection;
        private string _queueName;
        private ITransaction _transaction = default!;

        public Queue(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _connection = _connectionFactory.CreateConnection();
            _queueName = _connectionFactory.Configuration.GetQueueName();
        }

        public void BeginTranscation(CancellationToken cancellationToken = default)
        {
            _transaction = _connection.GetDatabase().CreateTransaction();
        }

        public bool Execute(CancellationToken cancellationToken = default)
        {
            return _transaction.Execute();
        }

        public void Enqueue(string value, CancellationToken cancellationToken = default)
        {
            _connection.GetDatabase().ListLeftPush(_queueName, value);
        }

        public string Dequeue(CancellationToken cancellationToken = default)
        {
            var result = _connection.GetDatabase().ListRightPop(_queueName);
            return result.ToString();
        }

        public IList<string> GetAllItems(CancellationToken cancellationToken = default)
        {
            var redisValues = _connection.GetDatabase().ListRange(_queueName, 0, -1);
            var result = redisValues.Select(x => x.ToString()).ToList();
            return result;
        }

        public long GetQueueLength(CancellationToken cancellationToken = default)
        {
            return _connection.GetDatabase().ListLength(_queueName);
        }

        public int Remove(string value, long count = 1, CancellationToken cancellationToken = default)
        {
            var removeItems = _connection.GetDatabase().ListRemove(_queueName, value.ToString(), count);
            return Convert.ToInt32(removeItems);
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
        ~Queue()
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
