using MediatR;
using WorkerService.Core.Behaviors;

namespace WorkerService.Core.Features.Groups.Commands
{
    public record DeleteGroupCommand : IRequest<int>, ILoggingTransaction
    {
        public int Id { get; }

        public DeleteGroupCommand(int id)
        {
            Id = id;
        }
    }
}
