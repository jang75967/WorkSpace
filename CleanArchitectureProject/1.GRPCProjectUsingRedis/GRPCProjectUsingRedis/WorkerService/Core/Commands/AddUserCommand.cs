using Domain.Entities;
using LanguageExt;
using MediatR;

namespace WorkerService.Core.Commands
{
    public record AddUserCommand(User User) : IRequest<Option<User>>;
}
