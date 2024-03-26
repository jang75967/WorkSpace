namespace RedisLibrary
{
    public interface IQueue : IDisposable
    {
        // 여긴 건드리지 않기?
        public void BeginTranscation(CancellationToken cancellationToken = default);
        public bool Execute(CancellationToken cancellationToken = default);
        // 여긴 건드리지 않기?
        long GetQueueLength(CancellationToken cancellationToken = default);
        void Enqueue(string value, CancellationToken cancellationToken = default);
        string Dequeue(CancellationToken cancellationToken = default);
        int Remove(string value, long count = 1, CancellationToken cancellationToken = default);
        IList<string> GetAllItems(CancellationToken cancellationToken = default);
    }
}
