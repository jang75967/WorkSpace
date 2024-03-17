using Application;
using Domain.Entities;
using LanguageExt;
using static LanguageExt.Prelude;

namespace WorkerService.Services
{
    public class UsersService : IUserService
    {
        public async Task<IEnumerable<Option<User>>> GetUsers()
        {
            return await Task.FromResult(new List<Option<User>>()
            {
                Some(new User
                {
                    Id = 1,
                    Email = "jdg1@gmail.com",
                    Name = "jdg1",
                }),
                Some(new User
                {
                    Id = 2,
                    Email = "jdg2@gmail.com",
                    Name = "jdg2",
                }),
                Some(new User
                {
                    Id = 3,
                    Email = "jdg3@gmail.com",
                    Name = "jdg3",
                }),
                None,
            });
        }
    }
}
