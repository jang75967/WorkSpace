using Domain.Entities;
using LanguageExt;
using MediatR;

namespace GrpcServiceUsingRedis.Queries
{
    public record GetUsersQuery() : IRequest<IEnumerable<Option<User>>>;
}
