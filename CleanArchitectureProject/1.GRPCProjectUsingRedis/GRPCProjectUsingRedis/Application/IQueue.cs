namespace Application
{
    public interface IQueue : IDisposable
    {
        public Task BeginTranscationAsync(CancellationToken cancellationToken = default);
        public Task<bool> ExecuteAsync(CancellationToken cancellationToken = default);
        public Task<long> EnqueueAsync(string value, CancellationToken cancellationToken = default);
        public Task<string> DequeueAsync(CancellationToken cancellationToken = default);
        public Task<long> GetQueueLengthAsync(CancellationToken cancellationToken = default);
        public Task<IList<string>> GetAllItemsAsync(CancellationToken cancellationToken = default);
        public Task<int> RemoveAsync(string value, long count = 1, CancellationToken cancellationToken = default);
    }
}
