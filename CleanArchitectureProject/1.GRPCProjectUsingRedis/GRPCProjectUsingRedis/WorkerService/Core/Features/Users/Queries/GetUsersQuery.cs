using Domain.Entities;
using LanguageExt;
using MediatR;

namespace WorkerService.Core.Features.Users.Queries
{
    public record GetUsersQuery() : IRequest<IEnumerable<Option<User>>>;
}
