using Application.Persistences;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EFCore.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly ApplicationDbContext _dbContext;
    public GroupRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Group> CreateAsync(Group entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Groups.AddAsync(entity);
        await this.SaveChanges();
        return await GetAsync(entity.Id, cancellationToken);
    }

    public async Task<bool> DeleteAsync(Group entity, CancellationToken cancellationToken = default)
    {
        var findEntity = await GetAsync(entity.Id, cancellationToken);
        _dbContext.Groups.Remove(findEntity);
        return await this.SaveChanges() >= 1;
    }

    public async Task<IEnumerable<Group>> FindAllAsync(IEnumerable<long> ids, CancellationToken cancellationToken = default)
    {
        var users = await _dbContext.Groups.Where(group => ids.Contains(group.Id)).ToListAsync(cancellationToken);
        return users;
    }

    public async Task<IEnumerable<Group>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Groups.OrderBy(_ => _.Id).ToListAsync();
    }

    public async Task<Group> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        var group = await _dbContext.Groups.FindAsync(new object[] { id }, cancellationToken);
        if (group == null)
            throw new ArgumentNullException(nameof(Group));
        return group;
    }

    public async Task<IEnumerable<Group>> GetGroupsByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.MemberUserGroups
            .Where(_ => _.UserId == userId)
            .Include(_ => _.Group)
            .Select(_ => _.Group)
            .OrderBy(g => g.Id).ToListAsync();
    }

    public async Task<int> SaveChanges()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<Group> UpdateAsync(Group entity, CancellationToken cancellationToken = default)
    {
        var findEntity = await this.GetAsync(entity.Id, cancellationToken);
        if (findEntity == null)
            throw new NullReferenceException(nameof(entity));

        findEntity = entity;
        await _dbContext.SaveChangesAsync();
        return findEntity;
    }
}
