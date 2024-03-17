using Domain.Entities;
using LanguageExt;
using MediatR;

namespace GrpcServiceUsingRedis.Commands
{
    public record AddUserCommand(User User) : IRequest<Option<User>>;
}
