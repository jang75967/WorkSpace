using EntityFrameworkCore.Client.Features.Coaches.Commands;
using EntityFrameworkCore.Data;
using MediatR;

namespace EntityFrameworkCore.Client.Features.Coaches.Handlers;

public class UpdateCoachCommandHandler : IRequestHandler<UpdateCoachCommand, int>
{
    private readonly FootballLeagueDbContext _dbContext;
    public UpdateCoachCommandHandler(FootballLeagueDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Handle(UpdateCoachCommand request, CancellationToken cancellationToken)
    {
        var updateCoach = await _dbContext.Coaches.FindAsync(request.Id);

        updateCoach.Name = request.Name;
        updateCoach.CreatedDate = DateTime.UtcNow;

        _dbContext.Coaches.Update(updateCoach);

        await _dbContext.SaveChangesAsync();

        return updateCoach.Id;
    }
}
