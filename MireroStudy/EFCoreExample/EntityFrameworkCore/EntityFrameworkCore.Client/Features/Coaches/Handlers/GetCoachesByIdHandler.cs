using EntityFrameworkCore.Client.Features.Coaches.Queries;
using EntityFrameworkCore.Data;
using EntityFrameworkCore.Domain;
using MediatR;

namespace EntityFrameworkCore.Client.Features.Coaches.Handlers;

public class GetCoachesByIdHandler : IRequestHandler<GetCoachByIdQuery, Coach>
{
    private readonly FootballLeagueDbContext _dbContext;

    public GetCoachesByIdHandler(FootballLeagueDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Coach> Handle(GetCoachByIdQuery request, CancellationToken cancellationToken)
    => await _dbContext.Coaches.FindAsync(request.id);
}
