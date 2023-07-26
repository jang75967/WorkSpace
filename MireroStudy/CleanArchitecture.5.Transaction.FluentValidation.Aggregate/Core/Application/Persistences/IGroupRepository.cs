using Domain.Entities;
using EntityGroup = Domain.Entities.Group;
namespace Application.Persistences;

public interface IGroupRepository : IBaseRepository<EntityGroup>
{
    public Task<IEnumerable<Group>> GetGroupsByUserIdAsync(long userId, CancellationToken cancellationToken=default);
}
