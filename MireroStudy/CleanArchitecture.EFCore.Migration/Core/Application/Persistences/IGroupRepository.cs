using EntityGroup = Domain.Entities.Group;
namespace Application.Persistences;

public interface IGroupRepository : IBaseRepository<EntityGroup>
{
    public Task<IEnumerable<EntityGroup>> GetUserJoinedGroups(Guid userId);
}
