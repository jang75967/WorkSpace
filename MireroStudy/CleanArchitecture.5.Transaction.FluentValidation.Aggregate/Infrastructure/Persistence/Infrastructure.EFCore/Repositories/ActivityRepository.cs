using Application.Persistences;
using Domain.Entities.ActivityAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EFCore.Repositories;

public class ActivityRepository : IActivityRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ActivityRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Activity> CreateAsync(Activity entity, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Activities.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync();
        
        return result.Entity;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var findEntity = await GetAsync(id, cancellationToken);

        if (findEntity != null)
        {
            _dbContext.Activities.Remove(findEntity);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<Activity>> FindAllAsync(IEnumerable<long> ids, CancellationToken cancellationToken = default)
    {
        var activities = await _dbContext.Activities.Where(av => ids.Contains(av.Id)).ToListAsync(cancellationToken);
        return activities;
    }

    public async Task<IEnumerable<Activity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Activities.Include(_ => _.Expenses).Include(_ => _.Attendees).OrderBy(_ => _.Id).ToListAsync();
    }

    public async Task<Activity> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        var activity = await _dbContext.Activities.Include(_ => _.Expenses).Include(_ => _.Attendees).FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (activity == null)
            throw new ArgumentNullException(nameof(Activity));
        return activity;
    }

    public async Task<Activity> UpdateAsync(Activity entity, CancellationToken cancellationToken = default)
    {
        var result = _dbContext.Activities.Update(entity).Entity;
        await _dbContext.SaveChangesAsync();
        return result;
    }
}
