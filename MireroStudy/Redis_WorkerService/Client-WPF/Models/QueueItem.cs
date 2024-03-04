namespace Client_WPF.Models
{
    public class QueueItem
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public QueueItem(int id, string value)
        {
            Id = id;
            Value = value;
        }
    }
}
