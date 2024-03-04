using MediatR;

namespace CQRSExample.Commands;

public record DeleteProductCommand : IRequest<int>
{
    public int Id { get;  }

    public DeleteProductCommand(int id)
    {
        Id = id;
    }
}
