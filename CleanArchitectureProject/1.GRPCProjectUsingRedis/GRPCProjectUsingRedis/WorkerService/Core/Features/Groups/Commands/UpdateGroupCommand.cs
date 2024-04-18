using Domain.Entities;
using LanguageExt;
using MediatR;
using WorkerService.Core.Behaviors;

namespace WorkerService.Core.Features.Groups.Commands
{
    public record UpdateGroupCommand(Group Group) : IRequest<Option<Group>>, ILoggingTransaction;
}
