using Application;
using Domain.Entities;
using LanguageExt;
using static LanguageExt.Prelude;

namespace InfraStructrue.Data.Repositories
{
    public class FakeRepsitory : IUserRepository
    {
        private List<Option<User>> _users = new List<Option<User>>()
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
        };

        public async Task<IEnumerable<Option<User>>> GetUsersAsync()
        {
            return await Task.FromResult(_users);
        }

        public async Task<Option<User>> GetUserByIdAsync(int id)
        {
            // id 일치하면 true, Option이 none이면 false 반환
            return await Task.FromResult(_users.FirstOrDefault(userOption => userOption.Match(Some: user => user.Id == id, None: () => false)));
        }

        public async Task AddUserAsync(User user)
        {
            _users.Add(Some(user));
            await Task.CompletedTask;
        }

        public async Task DeleteUserAsync(int id)
        {
            // id 일치하면 true, Option이 none이면 false 반환
            var userToRemoveOption = _users.FirstOrDefault(userOption => userOption.Match(
                Some: user => user.Id == id,
                None: () => false));

            if (userToRemoveOption.IsSome)
            {
                // Some일 경우, 해당 Option<User>를 리스트에서 제거
                _users = _users.Where(userOption => !userOption.Equals(userToRemoveOption)).ToList();
            }
            else
            {
                // None일 경우, 예외를 던짐
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            await Task.CompletedTask;
        }
    }
}
