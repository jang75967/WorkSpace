using Domain.Entities;

namespace Application;

public interface IUserService
{
    public Task<IEnumerable<User>> GetUsers();
}