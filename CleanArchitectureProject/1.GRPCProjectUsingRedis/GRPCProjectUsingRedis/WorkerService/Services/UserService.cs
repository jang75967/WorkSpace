﻿using Application;
using Domain.Entities;
using LanguageExt;

namespace WorkerService.Services
{
    public class UserService : IUserService
    {
        private readonly IFakeRepository _fakeRepository;

        public UserService(IFakeRepository fakeRepository)
        {
            _fakeRepository = fakeRepository;
        }

        public async Task<IEnumerable<Option<User>>> GetUsers()
        {
            return await _fakeRepository.GetUsersAsync();
        }

        public async Task<Option<User>> GetUserById(int id)
        {
            return await _fakeRepository.GetUserByIdAsync(id);
        }

        public async Task AddUser(User user)
        {
            await _fakeRepository.AddUserAsync(user);
        }

        public async Task DeleteUser(int id)
        {
            await _fakeRepository.DeleteUserAsync(id);
        }
    }
}
