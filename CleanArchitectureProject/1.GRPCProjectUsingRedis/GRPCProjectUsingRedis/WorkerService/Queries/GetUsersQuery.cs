using Domain.Entities;
using LanguageExt;
using MediatR;

namespace WorkerService.Queries
{
    public record GetUsersQuery() : IRequest<IEnumerable<Option<User>>>;
}
