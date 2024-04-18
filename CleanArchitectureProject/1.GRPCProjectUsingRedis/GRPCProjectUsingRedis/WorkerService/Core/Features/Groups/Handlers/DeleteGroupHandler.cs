using Application.Persistences;
using Domain.Entities;
using MediatR;
using WorkerService.Core.Features.Groups.Commands;

namespace WorkerService.Core.Features.Groups.Handlers
{
    public class DeleteGroupHandler : IRequestHandler<DeleteGroupCommand, int>
    {
        private readonly IBaseRepository<Group> _groupRepository;
        private readonly IQueue _queue;

        public DeleteGroupHandler(IBaseRepository<Group> groupRepository, IQueue queue)
        {
            _groupRepository = groupRepository;
            _queue = queue;
        }

        public async Task<int> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
        {
            var result = await _groupRepository.GetByIdAsync(request.Id, cancellationToken).MatchAsync(
                Some: async group =>
                {
                    // grpc response
                    await _groupRepository.DeleteAsync(group.Id, cancellationToken);

                    // queue service
                    //_queue.Dequeue(cancellationToken);
                    await _queue.DequeueAsync(cancellationToken);
                    return group.Id;
                },
                None: () =>
                {
                    return Task.FromResult(default(int));
                });

            return result;
        }
    }
}
