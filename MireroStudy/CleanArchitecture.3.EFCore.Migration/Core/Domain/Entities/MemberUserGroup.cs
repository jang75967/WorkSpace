namespace Domain.Entities;

public class MemberUserGroup : BaseEntity
{
    public long UserId { get; set; }
    public long GroupId { get; set; }
    public User User { get; set; } = default!;
    public Group Group { get; set; } = default!;

}
