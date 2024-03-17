using Application;
using Domain.Entities;

namespace GRPCProejctUsingRedis.Services
{
    public class UsersService : IUserService
    {
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await Task.FromResult(new List<User>()
            {
                new User
                {
                    Id = 1,
                    Email = "jdg1@gmail.com",
                    Name = "jdg1",
                },
                new User
                {
                    Id = 2,
                    Email = "jdg2@gmail.com",
                    Name = "jdg2",
                },
                new User
                {
                    Id = 3,
                    Email = "jdg3@gmail.com",
                    Name = "jdg3",
                },
            });
        }
    }
}
