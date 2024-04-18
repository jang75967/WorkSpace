using Domain.Entities;
using LanguageExt;
using MediatR;
using WorkerService.Core.Behaviors;

namespace WorkerService.Core.Features.Users.Commands
{
    public record UpdateUserCommand(User User) : IRequest<Option<User>>, ILoggingTransaction;
}
