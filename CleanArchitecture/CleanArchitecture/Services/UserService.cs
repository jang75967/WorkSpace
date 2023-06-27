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
            },
            new User
            {
                Id = 2,
                Email = "mirero2@mail.com",
                Name = "mirero2",
            },
            new User
            {
                Id = 3,
                Email = "mirero3@mail.com",
                Name = "mirero3",
            },
            new User
            {
                Id = 4,
                Email = "mirero4@mail.com",
                Name = "mirero4",
            }
        });
    }
}
