using Domain.Entities;
using LanguageExt;

namespace Application
{
    public interface IUserService
    {
        public Task<IEnumerable<Option<User>>> GetUsers();
        public Task<Option<User>> GetUserById(int id);
        public Task AddUser(User user);
        public Task DeleteUser(int id);
    }
}
