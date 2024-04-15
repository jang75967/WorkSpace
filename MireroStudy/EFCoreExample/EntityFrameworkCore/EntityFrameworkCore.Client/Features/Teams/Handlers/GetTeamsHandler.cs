using EntityFrameworkCore.Client.Features.Teams.Queries;
using EntityFrameworkCore.Data;
using EntityFrameworkCore.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Client.Features.Teams.Handlers;

public class GetTeamsHandler : IRequestHandler<GetTeamsQuery, IEnumerable<Team>>
{
    private readonly FootballLeagueDbContext _dbContext;
    public GetTeamsHandler(FootballLeagueDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Team>> Handle(GetTeamsQuery request, CancellationToken cancellationToken)
        => await _dbContext.Teams.ToListAsync();
    
}
