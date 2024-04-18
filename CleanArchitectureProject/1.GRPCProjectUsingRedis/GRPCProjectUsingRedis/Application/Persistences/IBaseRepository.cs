using LanguageExt;

namespace Application.Persistences
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        IEnumerable<Option<TEntity>> GetAll(CancellationToken cancellationToken = default);
        Option<TEntity> GetById(int id, CancellationToken cancellationToken = default);
        void Add(TEntity entity, CancellationToken cancellationToken = default);
        void Update(TEntity newEntity, CancellationToken cancellationToken = default);
        void Delete(int id, CancellationToken cancellationToken = default);

        Task<IEnumerable<Option<TEntity>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Option<TEntity>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(TEntity newEntity, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
