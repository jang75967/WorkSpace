using MediatR;

namespace EntityFrameworkCore.Client.Features.Teams.Commands;

public record DeleteTeamCommand : IRequest<int>
{
    public int Id { get; }

    public DeleteTeamCommand(int id)
    {
        Id = id;
    }
}

