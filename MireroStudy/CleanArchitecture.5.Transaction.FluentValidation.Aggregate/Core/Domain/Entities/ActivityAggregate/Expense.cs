namespace Domain.Entities.ActivityAggregate;

public class Expense : BaseEntity
{
    public long ActivityId { get; set; }
    public float Payment { get; set; }
    public Activity Activity { get; set; } = default!;
}
