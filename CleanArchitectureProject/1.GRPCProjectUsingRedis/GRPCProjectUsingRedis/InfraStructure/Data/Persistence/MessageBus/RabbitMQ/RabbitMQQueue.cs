using Application;
using Domain.MessageBus.Configuration;
using Domain.Resilience;
using RabbitMQ.Client;
using System.Text;

namespace InfraStructure.Data.Persistence.MessageBus.RabbitMQ
{
    public class RabbitMQQueue : IQueue
    {
        private readonly RabbitMQConnection _connection;
        private readonly string _queueName;
        private IModel _model = default!;

        public RabbitMQQueue(IConfiguration configuration, RetryOption retryOption)
        {
            _connection = new RabbitMQConnection(configuration, retryOption);
            _connection.CreateConnection();
            _queueName = configuration.GetQueueName();
            _model = _connection.Model;
        }

        #region Synchronous

        public void BeginTransaction(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public bool Execute(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public long Enqueue(string value, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public string Dequeue(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public long GetQueueLength(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public IList<string> GetAllItems(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public int Remove(string value, long count = 1, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Asynchronous

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _model.TxSelect();
            await Task.CompletedTask;
        }

        public async Task<bool> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            _model.TxCommit();
            return await Task.FromResult(true);
        }

        public async Task<long> EnqueueAsync(string value, CancellationToken cancellationToken = default)
        {
            var body = Encoding.UTF8.GetBytes(value);

            _model.BasicPublish(
                exchange: "",
                routingKey: _queueName,
                basicProperties: null,
                body: body);

            return await Task.FromResult(0);
        }

        public async Task<string> DequeueAsync(CancellationToken cancellationToken = default)
        {
            BasicGetResult result = _model.BasicGet(_queueName, autoAck: true);

            return result is null
                ? throw new InvalidOperationException("Queue is  empty.")
                : await Task.FromResult(Encoding.UTF8.GetString(result.Body.ToArray()));
        }

        public async Task<long> GetQueueLengthAsync(CancellationToken cancellationToken = default)
        {
            var result = _model.MessageCount(_queueName);
            return await Task.FromResult(result);
        }

        public async Task<IList<string>> GetAllItemsAsync(CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(new List<string>());
        }

        public async Task<int> RemoveAsync(string value, long count = 1, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(0);
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
        ~RabbitMQQueue()
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
