using Domain.Entities;
using LanguageExt;
using MediatR;

namespace WorkerService.Core.Features.Groups.Queries
{
    public record GetGroupsQuery() : IRequest<IEnumerable<Option<Group>>>;
}
