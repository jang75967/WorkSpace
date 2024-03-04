using StackExchange.Redis;

namespace RedisLibrary;

public class Stream : IStream
{
    IConnectionFactory _connectionFactory;
    IConnectionMultiplexer _connection;

    public Stream(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        _connection = _connectionFactory.CreateConnection();
    }

    public async Task AddStreamAsync(string streamName, string streamField, string streamValue)
    {
        await _connection.GetDatabase().StreamAddAsync(streamName, streamField, streamValue);
    }

    public async Task<StreamEntry[]> GetAllStreamsAsync(string streamName)
    {
        var messages = await _connection.GetDatabase().StreamRangeAsync(streamName, minId: "-", maxId: "+");
        return messages!;
    }

    public async Task<long> RemoveAsync(string value, string streamName, long count = 1)
    {
        RedisValue[] redisValues = new RedisValue[] { value };
        var result = await _connection.GetDatabase().StreamDeleteAsync(streamName, redisValues);
        return result;
    }

    public async Task<string> StreamInfoAsync(string streamName)
    {
        var info = await _connection.GetDatabase().StreamInfoAsync(streamName);
        return $"Length: {info.Length}, FirstEntry.ID: {info.FirstEntry.Id}, LastEntry.Id: {info.LastEntry.Id}";
    }

    public async Task<bool> StreamCreateConsumerGroupAsync(string streamName, string groupName)
    {
        bool result = false;
        if (!(await _connection.GetDatabase().KeyExistsAsync(streamName)) 
            || (await _connection.GetDatabase().StreamGroupInfoAsync(streamName)).All(x => x.Name != groupName))
        {
            result = await _connection.GetDatabase().StreamCreateConsumerGroupAsync(streamName, groupName, "0-0", true);
        }
        return result;
    }

    public async Task<StreamEntry[]> StreamReadGroupAsync(string streamName, string groupName, string consumerName, int count)
    {
        var consumerMessage = await _connection.GetDatabase().StreamReadGroupAsync(streamName, groupName, consumerName, ">", count: count);
        return consumerMessage;
    }

    public async Task<StreamPendingMessageInfo[]> StreamPendingMessagesAsync(string streamName, string groupName, int count, string consumerName, string minId)
    {
        var pendingMessages = await _connection.GetDatabase().StreamPendingMessagesAsync(streamName, groupName, count:count, consumerName, minId: minId);
        return pendingMessages;
    }

    public async Task StreamAcknowledgeAsync(string streamName, string groupName, string pendingMessageId)
    {
        await _connection.GetDatabase().StreamAcknowledgeAsync(streamName, groupName, pendingMessageId);
    }

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.
            _connection.Dispose();

            disposedValue = true;
        }
    }

    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    ~Stream()
    {
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        Dispose(false);
    }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        Dispose(true);
        // TODO: uncomment the following line if the finalizer is overridden above.
        GC.SuppressFinalize(this);
    }
    #endregion
}
