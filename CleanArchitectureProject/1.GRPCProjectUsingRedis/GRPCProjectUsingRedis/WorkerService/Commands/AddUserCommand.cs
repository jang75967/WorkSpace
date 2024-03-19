using Domain.Entities;
using LanguageExt;
using MediatR;

namespace WorkerService.Commands
{
    public record AddUserCommand(User User) : IRequest<Option<User>>;
}
