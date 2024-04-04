using Application;
using Domain.MessageBus.Configuration;
using Domain.Resilience;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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

            // Publisher Confirms를 사용하도록 채널 설정
            _model.ConfirmSelect();
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
            var body = Encoding.UTF8.GetBytes(value);

            _model.BasicPublish(
                exchange: "jdg.test.exchange",
                routingKey: _queueName,
                basicProperties: null,
                body: body);

            // 메시지가 RabbitMQ에 의해 수신되었는지 확인 (5초동안 대기)
            bool isConfirmed = _model.WaitForConfirms(new TimeSpan(0, 0, 5));

            if (!isConfirmed)
            {
                throw new Exception("Message could not be confirmed.");
            }

            return 0L;
        }

        public string Dequeue(CancellationToken cancellationToken = default)
        {
            BasicGetResult result = _model.BasicGet(_queueName, autoAck: false);

            if (result is null)
                throw new InvalidOperationException("Queue is empty.");

            try
            {
                var message = Encoding.UTF8.GetString(result.Body.ToArray());

                // autoAck 설정이 false 이므로 명시적으로 BasicAck 가 호출되어야 큐에서 제거
                _model.BasicAck(
                    deliveryTag: result.DeliveryTag,
                    multiple: false);

                return message;
            }
            catch
            {
                _model.BasicNack(
                    deliveryTag: result.DeliveryTag,
                    multiple: false,
                    requeue: true);

                throw;
            }
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
            // 성능상의 이유로 트랜잭션 권장하지 않음
            //_model.TxSelect();
            await Task.CompletedTask;
        }

        public async Task<bool> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            // 성능상의 이유로 트랜잭션 권장하지 않음
            //_model.TxCommit();
            return await Task.FromResult(true);
        }

        public async Task<long> EnqueueAsync(string value, CancellationToken cancellationToken = default)
        {
            var body = Encoding.UTF8.GetBytes(value);

            await Task.Run(() =>
            {
                _model.BasicPublish(
                    exchange: "jdg.test.exchange",
                    routingKey: _queueName,
                    basicProperties: null,
                    body: body);
            }, cancellationToken);

            // 메시지가 RabbitMQ에 의해 수신되었는지 확인 (5초동안 대기)
            bool isConfirmed = _model.WaitForConfirms(new TimeSpan(0, 0, 5));

            if (!isConfirmed)
            {
                throw new Exception("Message could not be confirmed.");
            }

            return await Task.FromResult(0L);
        }

        public async Task<string> DequeueAsync(CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<string>();
            string message = "";

            var consumer = new EventingBasicConsumer(_model);
            consumer.Received += (model, eventArgs) =>
            {
                try
                {
                    var body = eventArgs.Body.ToArray();
                    message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"Recevied: {message}");

                    // 메시지 처리 후 ack를 보내 큐에서 메시지 제거
                    _model.BasicAck(
                        deliveryTag: eventArgs.DeliveryTag,
                        multiple: false);

                    // 메시지 처리가 완료되면 TaskCompletionSource를 통해 결과를 설정
                    tcs.TrySetResult(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                    _model.BasicNack(
                        deliveryTag: eventArgs.DeliveryTag,
                        multiple: false,
                        requeue: true);

                    // 오류 발생 시 TaskCompletionSource를 통해 예외를 설정
                    tcs.TrySetException(ex);
                }
                finally
                {
                    _model.BasicConsume(
                        queue: _queueName,
                        autoAck: false,
                        consumer: consumer);
                }
            };

            // 취소 요청이 들어오면 Task가 취소되도록 등록
            cancellationToken.Register(() => tcs.TrySetCanceled());

            return await tcs.Task;
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
