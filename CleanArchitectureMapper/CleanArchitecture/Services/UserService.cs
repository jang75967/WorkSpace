using Application;
using Domain.Entities;

namespace CleanArchitecture.Services;

public class UserService : IUserService
{
    public async Task<IEnumerable<User>> GetUsers()
    {
        return await Task.FromResult(new List<User>()
        {
            new User
            {
                Id = 1,
                Email = "mirero@mail.com",
                Name = "mirero",
            }
        });
    }
}
