using EntityFrameworkCore.Client.Features.Teams.Commands;
using EntityFrameworkCore.Data;
using MediatR;

namespace EntityFrameworkCore.Client.Features.Teams.Handlers;

public class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand, int>
{
    private readonly FootballLeagueDbContext _dbContext;
    public DeleteTeamCommandHandler(FootballLeagueDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
    {
        var teamToDelete = await _dbContext.Teams.FindAsync(request.Id);

        if (teamToDelete != null)
        {
            _dbContext.Teams.Remove(teamToDelete);
            await _dbContext.SaveChangesAsync();
        }

        return teamToDelete!.TeamId;
    }
}
