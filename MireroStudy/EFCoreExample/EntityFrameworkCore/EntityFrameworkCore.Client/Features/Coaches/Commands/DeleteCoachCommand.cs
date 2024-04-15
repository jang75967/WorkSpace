using MediatR;

namespace EntityFrameworkCore.Client.Features.Coaches.Commands;

public record DeleteCoachCommand : IRequest<int>
{
    public int Id { get; }

    public DeleteCoachCommand(int id)
    {
        Id = id;
    }
}

