using MediatR;

namespace CQRSExample.Commands;

public record UpdateProductCommand : IRequest<int>
{
    public int Id { get; }
    public string Name { get; }

    public UpdateProductCommand(int id, string name)
    {
        Id = id;
        Name = name;
    }
}