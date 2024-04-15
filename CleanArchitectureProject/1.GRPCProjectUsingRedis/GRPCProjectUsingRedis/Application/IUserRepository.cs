using Domain.Entities;
using LanguageExt;

namespace Application
{
    public interface IUserRepository
    {
        public IEnumerable<Option<User>> GetUsers(CancellationToken cancellationToken = default);
        public Option<User> GetUserById(int id, CancellationToken cancellationToken = default);
        public void AddUser(User user, CancellationToken cancellationToken = default);
        public void UpdateUser(int id, User newUser, CancellationToken cancellationToken = default);
        public void DeleteUser(int id, CancellationToken cancellationToken = default);

        public Task<IEnumerable<Option<User>>> GetUsersAsync(CancellationToken cancellationToken = default);
        public Task<Option<User>> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
        public Task AddUserAsync(User user, CancellationToken cancellationToken = default);
        public Task UpdateUserAsync(int id, User newUser, CancellationToken cancellationToken = default);
        public Task DeleteUserAsync(int id, CancellationToken cancellationToken = default);
    }
}
