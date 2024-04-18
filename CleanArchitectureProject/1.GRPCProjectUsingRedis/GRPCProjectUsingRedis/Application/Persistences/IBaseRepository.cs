using LanguageExt;

namespace Application.Persistences
{
    public interface IBaseRepository<T> where T : class
    {
        IEnumerable<Option<T>> GetAll(CancellationToken cancellationToken = default);
        Option<T> GetById(int id, CancellationToken cancellationToken = default);
        void Add(T entity, CancellationToken cancellationToken = default);
        void Update(T newEntity, CancellationToken cancellationToken = default);
        void Delete(int id, CancellationToken cancellationToken = default);

        Task<IEnumerable<Option<T>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Option<T>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T newEntity, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
