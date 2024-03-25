using Application;
using Domain.Entities;
using LanguageExt;
using static LanguageExt.Prelude;

namespace GRPCProejctUsingRedis.Services
{
    public class UserService : IUserService
    {
        private static List<Option<User>> _users = default!;

        public UserService()
        {
            _users = new List<Option<User>>()
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
        }

        public async Task<IEnumerable<Option<User>>> GetUsersAsync()
        {
            return await Task.FromResult(_users);
        }

        public async Task<Option<User>> GetUserByIdAsync(int id)
        {
            // id ��ġ�ϸ� true, Option�� none�̸� false ��ȯ
            return await Task.FromResult(_users.FirstOrDefault(userOption => userOption.Match(Some: user => user.Id == id, None: () => false)));
        }

        public async Task AddUserAsync(User user)
        {
            _users.Add(user);
            await Task.CompletedTask;
        }

        public async Task DeleteUserAsync(int id)
        {
            // id ��ġ�ϸ� true, Option�� none�̸� false ��ȯ
            var userToRemoveOption = _users.FirstOrDefault(userOption => userOption.Match(user => user.Id == id, () => false));

            // userToRemoveOption �� Some�� ��� _users ����Ʈ���� �ش� User�� ����, None�� ��� ����
            userToRemoveOption.Match(userToRemove =>
            {
                _users.Remove(userToRemoveOption);
                return Unit.Default;
            },
            () =>
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            });

            await Task.CompletedTask;
        }
    }
}
