namespace Client_WPFStream.Models;

public class StreamItem
{
    public string Id { get; set; }
    public string StreamName { get; set; }
    public string Field { get; set; }
    public string Value { get; set; }

    public StreamItem(string id, string streamName, string field, string value)
    {
        Id = id;
        StreamName = streamName;
        Field = field;
        Value = value;
    }
}
