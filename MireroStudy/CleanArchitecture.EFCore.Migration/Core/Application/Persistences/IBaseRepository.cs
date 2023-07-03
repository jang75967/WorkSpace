namespace Application.Persistences;

public interface IBaseRepository<T> where T : class
{
    T Create(T entity);
    T Update(T entity);
    T Delete(T entity);
    T CreateAsync(T entity);
    T UpdateAsync(T entity);
    T DeleteAsync(T entity);
    Task<T> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> FindAllAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
}