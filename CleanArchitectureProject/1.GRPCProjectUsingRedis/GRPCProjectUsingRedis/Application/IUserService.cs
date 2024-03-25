using Domain.Entities;
using LanguageExt;

namespace Application
{
    public interface IUserService
    {
        public Task<IEnumerable<Option<User>>> GetUsersAsync();
        public Task<Option<User>> GetUserByIdAsync(int id);
        public Task AddUserAsync(User user);
        public Task DeleteUserAsync(int id);
    }
}
