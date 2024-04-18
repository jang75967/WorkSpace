using Application.Persistences;
using Domain.Entities;
using LanguageExt;
using MediatR;
using WorkerService.Core.Features.Groups.Commands;

namespace WorkerService.Core.Features.Groups.Handlers
{
    public class UpdateGroupHandler : IRequestHandler<UpdateGroupCommand, Option<Group>>
    {
        private readonly IBaseRepository<Group> _groupRepository;

        public UpdateGroupHandler(IBaseRepository<Group> groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<Option<Group>> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
        {
            // grpc response
            await _groupRepository.UpdateAsync(request.Group, cancellationToken);

            return Option<Group>.Some(request.Group);
        }
    }
}
