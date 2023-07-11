using Application.Persistences;
using Azure.Core;
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

    public async Task<User> CreateAsync(User entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(entity);
        await this.SaveChanges();
        return await GetAsync(entity.Id, cancellationToken);
    }

    public async Task<bool> DeleteAsync(User entity, CancellationToken cancellationToken = default)
    {
        var findEntity = await GetAsync(entity.Id, cancellationToken);
        _dbContext.Remove(findEntity);
        return await this.SaveChanges() >= 1;
    }

    public async Task<IEnumerable<User>> FindAllAsync(IEnumerable<long> ids, CancellationToken cancellationToken = default)
    {
        var users = await _dbContext.Users.Where(user => ids.Contains(user.Id)).ToListAsync(cancellationToken);
        return users;
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.OrderBy(_=> _.Id).ToListAsync();
    }

    public async Task<User> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FindAsync(new object[] { id }, cancellationToken);
        if (user == null)
            throw new ArgumentNullException(nameof(User));
        return user;
    }

    public async Task<IEnumerable<User>> GetGroupMembers(long grouId, CancellationToken cancellationToken = default)
    {
        var a = await _dbContext.MemberUserGroups
            .Where(_ => _.GroupId == grouId).Include(_ => _.User).ToListAsync();
        return await _dbContext.MemberUserGroups
            .Where(_ => _.GroupId == grouId)
            .Include(_ => _.User)
            .Select(_ => _.User)
            .OrderBy(_ => _.Id).ToListAsync();
    }


    public async Task<int> SaveChanges()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<User> UpdateAsync(User entity, CancellationToken cancellationToken = default)
    {
        var findEntity = await this.GetAsync(entity.Id, cancellationToken);
        if (findEntity == null)
            throw new NullReferenceException(nameof(entity));

        findEntity = entity;
        await _dbContext.SaveChangesAsync();
        return findEntity;
    }
}
