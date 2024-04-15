using EntityFrameworkCore.Client.Features.Teams.Commands;
using EntityFrameworkCore.Data;
using MediatR;

namespace EntityFrameworkCore.Client.Features.Teams.Handlers;

public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, int>
{
    private readonly FootballLeagueDbContext _dbContext;
    public UpdateTeamCommandHandler(FootballLeagueDbContext dbContext)
    {
        _dbContext = dbContext;    
    }

    public async Task<int> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
    {
        var updateTeam = await _dbContext.Teams.FindAsync(request.Id);

        updateTeam.Name = request.Name;
        updateTeam.CreatedDate = DateTime.UtcNow;

        _dbContext.Teams.Update(updateTeam);

        await _dbContext.SaveChangesAsync();

        return updateTeam.TeamId;
    }
}
