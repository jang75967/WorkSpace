using MediatR;

namespace WorkerService.Commands
{
    public record DeleteUserCommand : IRequest<int>
    {
        public int Id { get; }

        public DeleteUserCommand(int id)
        {
            Id = id;
        }
    }
}
