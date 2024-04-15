using EntityFrameworkCore.Client.Features.Teams.Commands;
using EntityFrameworkCore.Data;
using MediatR;

namespace EntityFrameworkCore.Client.Features.Teams.Handlers;

public class AddTeamHandler : IRequestHandler<AddTeamCommand>
{
    private readonly FootballLeagueDbContext _dbContext;

    public AddTeamHandler(IMediator mediator, FootballLeagueDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(AddTeamCommand request, CancellationToken cancellationToken)
    {
        _dbContext.Teams.Add(request.Team);
        await _dbContext.SaveChangesAsync();
    }
}

