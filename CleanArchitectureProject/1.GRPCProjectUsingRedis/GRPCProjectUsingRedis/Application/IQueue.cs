namespace Application
{
    public interface IQueue : IDisposable
    {
        public void BeginTranscation(CancellationToken cancellationToken = default);
        public bool Execute(CancellationToken cancellationToken = default);
        public long Enqueue(string value, CancellationToken cancellationToken = default);
        public string Dequeue(CancellationToken cancellationToken = default);
        public long GetQueueLength(CancellationToken cancellationToken = default);
        public IList<string> GetAllItems(CancellationToken cancellationToken = default);
        public int Remove(string value, long count = 1, CancellationToken cancellationToken = default);

        public Task BeginTranscationAsync(CancellationToken cancellationToken = default);
        public Task<bool> ExecuteAsync(CancellationToken cancellationToken = default);
        public Task<long> EnqueueAsync(string value, CancellationToken cancellationToken = default);
        public Task<string> DequeueAsync(CancellationToken cancellationToken = default);
        public Task<long> GetQueueLengthAsync(CancellationToken cancellationToken = default);
        public Task<IList<string>> GetAllItemsAsync(CancellationToken cancellationToken = default);
        public Task<int> RemoveAsync(string value, long count = 1, CancellationToken cancellationToken = default);
    }
}
