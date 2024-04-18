using Domain.Entities;
using LanguageExt;
using MediatR;
using WorkerService.Core.Behaviors;

namespace WorkerService.Core.Features.Groups.Commands
{
    public record AddGroupCommand(Group Group) : IRequest<Option<Group>>, ILoggingTransaction;
}
