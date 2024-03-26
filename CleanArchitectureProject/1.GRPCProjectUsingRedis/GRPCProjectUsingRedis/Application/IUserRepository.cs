using Domain.Entities;
using LanguageExt;

namespace Application
{
    public interface IUserRepository
    {
        public Task<IEnumerable<Option<User>>> GetUsersAsync(CancellationToken cancellationToken = default);
        public Task<Option<User>> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
        public Task AddUserAsync(User user, CancellationToken cancellationToken = default);
        public Task DeleteUserAsync(int id, CancellationToken cancellationToken = default);
    }
}
