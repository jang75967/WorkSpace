using Application.Persistences;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EFCore.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;
    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
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
        return await _dbContext.Users.OrderBy(_=> _.Name).ToListAsync();
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
