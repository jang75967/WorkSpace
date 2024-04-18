using Application.Persistences;
using Domain.Entities;
using InfraStructure.Data.Persistence.EFCore;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;

namespace InfraStructure.Data.Repositories
{
    public class EFCoreUserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EFCoreUserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Synchronous

        public IEnumerable<Option<User>> GetAll(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Option<User> GetById(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Add(User user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Update(User newUser, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Asynchronous

        public async Task<IEnumerable<Option<User>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var users = await _dbContext.Users.OrderBy(o => o.Id).ToListAsync();
            return users.Select(Option<User>.Some);
        }

        public async Task<Option<User>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.FindAsync(new object[] { id }, cancellationToken);
            return user is not null ? Option<User>.Some(user) : Option<User>.None;
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync();
            await Task.CompletedTask; 
        }

        public async Task UpdateAsync(User newUser, CancellationToken cancellationToken = default)
        {
            var result = _dbContext.Users.Update(newUser).Entity;
            await _dbContext.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var removeUserOPtion = await GetByIdAsync(id, cancellationToken);

            removeUserOPtion.Match(
                Some: user =>
                {
                    _dbContext.Users.Remove(user);
                    _dbContext.SaveChangesAsync();
                },
                None: () =>
                {
                    throw new KeyNotFoundException($"No user found with ID {id}");
                });

            await Task.CompletedTask;
        }

        #endregion
    }
}
