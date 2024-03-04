using StackExchange.Redis;

namespace RedisLibrary
{
    public class Queue : IQueue
    {
        IConnectionFactory _connectionFactory;
        IConnectionMultiplexer _connection;
        string _queueName;

        public Queue(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _connection = _connectionFactory.CreateConnection();
            _queueName = _connectionFactory.Configuration.GetQueueName();
        }

        public string Dequeue()
        {
            var result = _connection.GetDatabase().ListRightPop(_queueName);
            return result.ToString();
        }

        public void Enqueue(string value)
        {
            _connection.GetDatabase().ListLeftPush(_queueName, value.ToString());
        }

        public IList<string> GetAllItems()
        {
            var redisValues = _connection.GetDatabase().ListRange(_queueName, 0, -1);
            var result = redisValues.Select(x => x.ToString()).ToList();
            return result;
        }


        public long GetQueueLength()
        {
            return _connection.GetDatabase().ListLength(_queueName);
        }

        public int Remove(string value, long count = 1)
        {
            var removeItems = _connection.GetDatabase().ListRemove(_queueName, value.ToString(), count);
            return Convert.ToInt32(removeItems);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
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
