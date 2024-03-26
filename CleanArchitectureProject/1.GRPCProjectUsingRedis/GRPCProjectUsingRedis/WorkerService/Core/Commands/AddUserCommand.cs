using Domain.Entities;
using LanguageExt;
using MediatR;
using WorkerService.Core.Behaviors;

namespace WorkerService.Core.Commands
{
    public record AddUserCommand(User User) : IRequest<Option<User>>, ILoggingTransaction;
}
