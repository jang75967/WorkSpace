using Domain.Attributes;
using Domain.Exceptions;

namespace Domain.Entities;

[AggregateRoot]
public class MemberUserGroup : BaseEntity
{
    public long UserId { get; set; }
    public long GroupId { get; set; }
    public User User { get; set; } = default!;
    public Group Group { get; set; } = default!;

    public MemberUserGroup() { }

    public MemberUserGroup(long userId, long groupId)
    {
        if (userId <= 0) throw new DomainException($"Invalid userId: '{nameof(userId)}'. UserId must be a positive number.");
        if (groupId <= 0) throw new DomainException($"Invalid groupId: '{nameof(groupId)}'. GroupId must be a positive number.");
        this.UserId = userId;
        this.GroupId = groupId;
    }

    public static MemberUserGroup Create(long userId, long groupId) => new MemberUserGroup(userId, groupId);
}
