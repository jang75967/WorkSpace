using EntityFrameworkCore.Client.Features.Coaches.Commands;
using EntityFrameworkCore.Client.Features.Teams.Commands;
using EntityFrameworkCore.Data;
using MediatR;

namespace EntityFrameworkCore.Client.Features.Coaches.Handlers;

public class AddCoachHandler : IRequestHandler<AddCoachCommand>
{
    private readonly FootballLeagueDbContext _dbContext;

    public AddCoachHandler(IMediator mediator, FootballLeagueDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(AddCoachCommand request, CancellationToken cancellationToken)
    {
        _dbContext.Coaches.Add(request.Coach);
        await _dbContext.SaveChangesAsync();
    }
}

