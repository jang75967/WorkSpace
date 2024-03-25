using Domain.Entities;
using LanguageExt;
using MediatR;

namespace WorkerService.Core.Queries
{
    public record GetUsersQuery() : IRequest<IEnumerable<Option<User>>>;
}
