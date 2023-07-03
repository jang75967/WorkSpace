using EntityGroup = Domain.Entities.Group;

namespace Infrastructure.Persistence.Common.EntityDatas;

public class GroupEntityDatas
{
    public static IEnumerable<EntityGroup> Initialize()
    {
        return new List<EntityGroup>()
        {
            new EntityGroup() { Id = 1, Name = "축구 동아리" },
            new EntityGroup() { Id = 2, Name = "농구 동아리" }

        };
    }
}
