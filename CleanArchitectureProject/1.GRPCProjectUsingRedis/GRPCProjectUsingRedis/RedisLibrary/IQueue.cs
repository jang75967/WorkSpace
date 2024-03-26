namespace RedisLibrary
{
    public interface IQueue : IDisposable
    {
        // 여긴 건드리지 않기?
        public void BeginTranscation();
        public bool Execute();
        // 여긴 건드리지 않기?
        long GetQueueLength();
        void Enqueue(string value);
        string Dequeue();
        int Remove(string value, long count = 1);
        IList<string> GetAllItems();
    }
}
