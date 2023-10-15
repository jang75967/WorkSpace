namespace Domain.Entities.ActivityAggregate;

public class Attendant : BaseEntity
{
    public long ActivityId { get; set; }
    public long UserId { get; set; }
    public Activity Activity { get; set; } = default!;
    public User User { get; set; } = default!;
}
