using StackExchange.Redis;

namespace RedisLibrary;

public interface IStream : IDisposable
{
    Task AddStreamAsync(string streamName, string streamField, string streamValue);
    Task<bool> StreamCreateConsumerGroupAsync(string streamName, string groupName);
    Task<StreamEntry[]> StreamReadGroupAsync(string streamName, string groupName, string consumerName, int count);
    Task<StreamPendingMessageInfo[]> StreamPendingMessagesAsync(string streamName, string groupName, int count, string consumerName, string minId);
    Task StreamAcknowledgeAsync(string streamName, string groupName, string pendingMessageId);
    Task<long> RemoveAsync(string value, string streamName, long count = 1);
    Task<string> StreamInfoAsync(string streamName);
    Task<StreamEntry[]> GetAllStreamsAsync(string streamName);
}
