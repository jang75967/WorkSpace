using Application.Persistences;
using Domain.Entities;
using MongoDB.Driver;

namespace Infrastructure.MongoDB.Repositories;

public class UserRepository : IUserRepository
{
    private IMongoCollection<User> _userCollection;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _userCollection = dbContext.CreateCollection<User>();
    }

    public User Create(User entity)
    {
        throw new NotImplementedException();
    }

    public Task<User> CreateAsync(User entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public User Delete(User entity)
    {
        throw new NotImplementedException();
    }

    public Task<User> DeleteAsync(User entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> FindAllAsync(IEnumerable<long> ids, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetAllGroupMembers(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetGroupMembers(long grouId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveChanges()
    {
        throw new NotImplementedException();
    }

    public User Update(User entity)
    {
        throw new NotImplementedException();
    }

    public Task<User> UpdateAsync(User entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    Task<bool> IBaseRepository<User>.DeleteAsync(User entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
