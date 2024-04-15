using MediatR;

namespace EntityFrameworkCore.Client.Features.Coaches.Commands;

public record UpdateCoachCommand : IRequest<int>
{
    public int Id { get; }
    public string Name { get; }

    public UpdateCoachCommand(int id, string name)
    {
        Id = id;
        Name = name;
    }
}