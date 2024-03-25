namespace Application
{
    public interface IQueueService
    {
        public Task Push(string input);
        public Task Pop();
    }
}
