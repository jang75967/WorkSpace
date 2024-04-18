using Application.Persistences;
using Domain.Entities;
using InfraStructure.Data.Persistence.EFCore;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace InfraStructure.Data.Repositories
{
    // 현재 사용 안함 (EFCoreRepository.cs 에서 공통으로 사용)
    public class EFCoreGroupRepository : IGroupRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EFCoreGroupRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Synchronous

        public IEnumerable<Option<Group>> GetAll(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Option<Group> GetById(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Add(Group group, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Update(Group newGroup, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Asynchronous

        public async Task<IEnumerable<Option<Group>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var groups = await _dbContext.Groups.OrderBy(o => o.Id).ToListAsync();
            return groups.Select(Option<Group>.Some);
        }

        public async Task<Option<Group>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var group = await _dbContext.Groups.FindAsync(new object[] { id }, cancellationToken);
            return group is not null ? Option<Group>.Some(group) : Option<Group>.None;
        }

        public async Task AddAsync(Group group, CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.Groups.AddAsync(group, cancellationToken);
            await _dbContext.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Group newGroup, CancellationToken cancellationToken = default)
        {
            var result = _dbContext.Groups.Update(newGroup).Entity;
            await _dbContext.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var removeGroupOption = await GetByIdAsync(id, cancellationToken);

            removeGroupOption.Match(
                Some: group =>
                {
                    _dbContext.Groups.Remove(group);
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
