namespace Application
{
    public interface IQueueService
    {
        public Task BeginTranscationAsync();
        public Task<bool> ExecuteAsync();
        public Task PushAsync(string input);
        public Task PopAsync();
    }
}
