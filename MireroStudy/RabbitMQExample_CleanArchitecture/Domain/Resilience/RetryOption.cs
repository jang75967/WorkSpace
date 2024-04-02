namespace Domain.Resilience;

public record RetryOption
{
    public int RetryCount { get; }
    public int RetryDelayMilliseconds { get; }

    public RetryOption(int retryCount = 1, int retryDelayMilliseconds = 10 * 1000)
    {
        RetryCount = retryCount;
        RetryDelayMilliseconds = retryDelayMilliseconds;
    }
}