using Application.Persistences;
using Domain.Entities;
using InfraStructure.Data.Persistence.EFCore;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace InfraStructure.Data.Repositories
{
    public class EFCoreRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public EFCoreRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        #region Synchronous

        public IEnumerable<Option<TEntity>> GetAll(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Option<TEntity> GetById(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Add(TEntity entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity newEntity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Asynchronous

        public async Task<IEnumerable<Option<TEntity>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var items = await _dbSet.OrderBy(o => o.Id).ToListAsync(cancellationToken);
            return items.Select(Option<TEntity>.Some);
        }

        public async Task<Option<TEntity>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var item = await _dbSet.FindAsync(new object[] { id }, cancellationToken);
            return item is not null ? Option<TEntity>.Some(item) : Option<TEntity>.None;
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            //await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(TEntity newEntity, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(newEntity);
            await Task.CompletedTask;
            //await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var option = await GetByIdAsync(id, cancellationToken);

            option.Match(
                Some: entity =>
                {
                    _dbSet.Remove(entity);
                    //_dbContext.SaveChangesAsync(cancellationToken);
                },
                None: () => throw new KeyNotFoundException($"No {typeof(TEntity).Name} found with ID {id}")
            );

            await Task.CompletedTask;
        }

        #endregion
    }
}
