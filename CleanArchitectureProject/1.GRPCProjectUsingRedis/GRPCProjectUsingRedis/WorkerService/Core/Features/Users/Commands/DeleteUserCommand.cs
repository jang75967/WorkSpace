using MediatR;
using WorkerService.Core.Behaviors;

namespace WorkerService.Core.Features.Users.Commands
{
    public record DeleteUserCommand : IRequest<int>, ILoggingTransaction
    {
        public int Id { get; }

        public DeleteUserCommand(int id)
        {
            Id = id;
        }
    }
}
