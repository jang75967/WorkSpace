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

    public User CreateAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public User Delete(User entity)
    {
        throw new NotImplementedException();
    }

    public User DeleteAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> FindAllAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await _userCollection.Find(_ => true).SortBy(_ => _.Name).ToListAsync();
        return result;
    }

    public Task<User> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetGroupMembers(Guid groupId)
    {
        throw new NotImplementedException();
    }

    public User Update(User entity)
    {
        throw new NotImplementedException();
    }

    public User UpdateAsync(User entity)
    {
        throw new NotImplementedException();
    }
}
