namespace RedisLibrary
{
    public interface IQueue : IDisposable
    {
        void BeginTranscation();
        bool Execute();
        long GetQueueLength();
        void Enqueue(string value);
        string Dequeue();
        int Remove(string value, long count = 1);
        IList<string> GetAllItems();
    }
}
