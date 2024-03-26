namespace Application
{
    public interface IQueueService
    {
        public Task BeginTranscationAsync(CancellationToken cancellationToken = default);
        public Task<bool> ExecuteAsync(CancellationToken cancellationToken = default);
        public Task PushAsync(string input, CancellationToken cancellationToken = default);
        public Task PopAsync(CancellationToken cancellationToken = default);
    }
}
