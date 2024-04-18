using Application.Persistences;
using Domain.Entities;
using LanguageExt;
using MediatR;
using WorkerService.Core.Features.Groups.Queries;

namespace WorkerService.Core.Features.Groups.Handlers
{
    public class GetGroupsHandler : IRequestHandler<GetGroupsQuery, IEnumerable<Option<Group>>>
    {
        private readonly IBaseRepository<Group> _groupRepository;

        public GetGroupsHandler(IBaseRepository<Group> groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<IEnumerable<Option<Group>>> Handle(GetGroupsQuery request, CancellationToken cancellationToken)
        {
            return await _groupRepository.GetAllAsync(cancellationToken);
        }
    }
}
