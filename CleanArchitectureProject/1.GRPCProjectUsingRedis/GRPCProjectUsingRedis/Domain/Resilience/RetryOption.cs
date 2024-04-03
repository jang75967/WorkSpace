namespace Domain.Resilience
{
    public record RetryOption
    {
        public int RetryCount { get; init; } = 1;
        public int RetryDelayMilliseconds { get; init; } = 10 * 1000;
    }
}
