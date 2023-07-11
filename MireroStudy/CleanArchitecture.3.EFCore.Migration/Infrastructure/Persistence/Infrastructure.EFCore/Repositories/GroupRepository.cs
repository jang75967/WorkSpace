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

    public Group Create(Group entity)
    {
        throw new NotImplementedException();
    }

    public Group CreateAsync(Group entity)
    {
        throw new NotImplementedException();
    }

    public Group Delete(Group entity)
    {
        throw new NotImplementedException();
    }

    public Group DeleteAsync(Group entity)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Group>> FindAllAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Group>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Groups.OrderBy(_ => _.Name).ToListAsync();
    }

    public Task<Group> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Group>> GetUserJoinedGroups(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Group Update(Group entity)
    {
        throw new NotImplementedException();
    }

    public Group UpdateAsync(Group entity)
    {
        throw new NotImplementedException();
    }
}
