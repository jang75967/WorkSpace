using Domain.Entities;
using LanguageExt;

namespace Application
{
    public interface IFakeRepository
    {
        Task<IEnumerable<Option<User>>> GetUsersAsync();
        Task<Option<User>> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task DeleteUserAsync(int id);
    }
}
