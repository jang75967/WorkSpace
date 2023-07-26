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
        var result = await _dbContext.Groups.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var findEntity = await GetAsync(id, cancellationToken);

        if(findEntity != null)
        {
            _dbContext.Groups.Remove(findEntity);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
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

    public async Task<Group> UpdateAsync(Group entity, CancellationToken cancellationToken = default)
    {
        var findEntity = await this.GetAsync(entity.Id, cancellationToken);
        if (findEntity == null)
            throw new NullReferenceException(nameof(entity));

        findEntity.Name = entity.Name;
        var result = _dbContext.Groups.Update(findEntity).Entity;
        await _dbContext.SaveChangesAsync();
        return result;
    }
}
