using Application.Persistences;
using Domain.Entities;
using LanguageExt;
using MediatR;
using WorkerService.Core.Features.Groups.Commands;

namespace WorkerService.Core.Features.Groups.Handlers
{
    public class AddGroupHandler : IRequestHandler<AddGroupCommand, Option<Group>>
    {
        private readonly IBaseRepository<Group> _groupRepository;
        private readonly IQueue _queue;

        public AddGroupHandler(IBaseRepository<Group> groupRepository, IQueue queue)
        {
            _groupRepository = groupRepository;
            _queue = queue;
        }

        public async Task<Option<Group>> Handle(AddGroupCommand request, CancellationToken cancellationToken)
        {
            // grpc response
            await _groupRepository.AddAsync(request.Group, cancellationToken);

            // queue service
            //_queue.Enqueue(request.Group.Name, cancellationToken);
            await _queue.EnqueueAsync(request.Group.Name, cancellationToken);

            return Option<Group>.Some(request.Group);
        }
    }
}
