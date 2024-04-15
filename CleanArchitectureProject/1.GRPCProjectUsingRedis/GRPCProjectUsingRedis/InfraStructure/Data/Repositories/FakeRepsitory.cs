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

        #region Synchronous

        public IEnumerable<Option<User>> GetUsers(CancellationToken cancellationToken = default)
        {
            return _users;
        }

        public Option<User> GetUserById(int id, CancellationToken cancellationToken = default)
        {
            return _users.FirstOrDefault(userOption => userOption.Match(Some: user => user.Id == id, None: () => false));
        }

        public void AddUser(User user, CancellationToken cancellationToken = default)
        {
            _users.Add(Some(user));
        }

        public void UpdateUser(int id, User newUser, CancellationToken cancellationToken = default)
        {
            var userToUpdateOption = _users.FirstOrDefault(userOption => userOption.Match(
                Some: user => user.Id == id,
                None: () => false));

            if (userToUpdateOption.IsSome)
            {
                _users = _users.Select(userOption => userOption.Match(
                    Some: user =>
                    {
                        if (user.Id == id)
                        {
                            return Option<User>.Some(newUser);
                        }
                        return Option<User>.Some(user);
                    },
                    None: () => Option<User>.None)).ToList();
            }
            else
            {
                // None일 경우, 예외를 던짐
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }
        }

        public void DeleteUser(int id, CancellationToken cancellationToken = default)
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
        }

        #endregion

        #region Asynchronous

        public async Task<IEnumerable<Option<User>>> GetUsersAsync(CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(_users);
        }

        public async Task<Option<User>> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            // id 일치하면 true, Option이 none이면 false 반환
            return await Task.FromResult(_users.FirstOrDefault(userOption => userOption.Match(Some: user => user.Id == id, None: () => false)));
        }

        public async Task AddUserAsync(User user, CancellationToken cancellationToken = default)
        {
            _users.Add(Some(user));
            await Task.CompletedTask;
        }

        public async Task UpdateUserAsync(int id, User newUser, CancellationToken cancellationToken = default)
        {
            var userToUpdateOption = _users.FirstOrDefault(userOption => userOption.Match(
                Some: user => user.Id == id,
                None: () => false));

            if (userToUpdateOption.IsSome)
            {
                _users = _users.Select(userOption => userOption.Match(
                    Some: user =>
                    {
                        if (user.Id == id)
                        {
                            return Option<User>.Some(newUser);
                        }
                        return Option<User>.Some(user);
                    },
                    None: () => Option<User>.None)).ToList();
            }
            else
            {
                // None일 경우, 예외를 던짐
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            await Task.CompletedTask;
        }

        public async Task DeleteUserAsync(int id, CancellationToken cancellationToken = default)
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

        #endregion
    }
}
