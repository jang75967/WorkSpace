using EntityFrameworkCore.Client.Features.Coaches.Queries;
using EntityFrameworkCore.Data;
using EntityFrameworkCore.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Client.Features.Coaches.Handlers;

public class GetCoachesHandler : IRequestHandler<GetCoachesQuery, IEnumerable<Coach>>
{
    private readonly FootballLeagueDbContext _dbContext;
    public GetCoachesHandler(FootballLeagueDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Coach>> Handle(GetCoachesQuery request, CancellationToken cancellationToken)
        => await _dbContext.Coaches.ToListAsync();

}
