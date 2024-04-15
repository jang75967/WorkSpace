using EntityFrameworkCore.Client.Features.Teams.Queries;
using EntityFrameworkCore.Data;
using EntityFrameworkCore.Domain;
using MediatR;

namespace EntityFrameworkCore.Client.Features.Teams.Handlers;

public class GetTeamsByIdHandler : IRequestHandler<GetTeamByIdQuery, Team>
{
    private readonly FootballLeagueDbContext _dbContext;
    public GetTeamsByIdHandler(FootballLeagueDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Team> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
    => await _dbContext.Teams.FindAsync(request.id);
}
