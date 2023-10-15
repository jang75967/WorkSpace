using Domain.Entities;

namespace Infrastructure.Persistence.Common.EntityDatas;

public class MemberUserGroupEntityDatas
{
    public static IEnumerable<MemberUserGroup> Initialize()
    {
        return new List<MemberUserGroup>()
        {
            new MemberUserGroup() { Id = 1, UserId = 1, GroupId = 1 },
            new MemberUserGroup() { Id = 2, UserId = 1, GroupId = 2 },
            new MemberUserGroup() { Id = 3, UserId = 2, GroupId = 1 },
            new MemberUserGroup() { Id = 4, UserId = 3, GroupId = 2 },
            new MemberUserGroup() { Id = 5, UserId = 4, GroupId = 1 },
        };
    }
}
