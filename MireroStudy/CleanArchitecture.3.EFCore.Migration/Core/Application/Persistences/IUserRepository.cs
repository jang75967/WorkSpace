using Domain.Entities;

namespace Application.Persistences;

public interface IUserRepository : IBaseRepository<User>
{
    public Task<IEnumerable<User>> GetGroupMembers(Guid groupId);
}
