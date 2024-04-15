using EntityFrameworkCore.Client.Features.Coaches.Commands;
using EntityFrameworkCore.Client.Features.Teams.Commands;
using EntityFrameworkCore.Data;
using MediatR;

namespace EntityFrameworkCore.Client.Features.Coaches.Handlers;

public class DeleteCoachCommandHandler : IRequestHandler<DeleteCoachCommand, int>
{
    private readonly FootballLeagueDbContext _dbContext;
    public DeleteCoachCommandHandler(FootballLeagueDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Handle(DeleteCoachCommand request, CancellationToken cancellationToken)
    {
        var coachToDelete = await _dbContext.Coaches.FindAsync(request.Id);

        if (coachToDelete != null)
        {
            _dbContext.Coaches.Remove(coachToDelete);
            await _dbContext.SaveChangesAsync();
        }

        return coachToDelete!.Id;
    }
}
