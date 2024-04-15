using MediatR;

namespace EntityFrameworkCore.Client.Features.Teams.Commands;

public record UpdateTeamCommand : IRequest<int>
{
    public int Id { get; }
    public string Name { get; }

    public UpdateTeamCommand(int id, string name)
    {
        Id = id;
        Name = name;
    }
}