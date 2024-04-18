using Application.Persistences;
using MediatR;
using WorkerService.Core.Features.Users.Commands;

namespace WorkerService.Core.Features.Users.Handlers
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, int>
    {
        private readonly IUserRepository _userRepository;
        private readonly IQueue _queue;

        public DeleteUserHandler(IUserRepository userRepository, IQueue queue)
        {
            _userRepository = userRepository;
            _queue = queue;
        }

        public async Task<int> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetByIdAsync(request.Id, cancellationToken).MatchAsync(
                Some: async user =>
                {
                    // grpc response
                    await _userRepository.DeleteAsync(user.Id, cancellationToken);

                    // queue service
                    //_queue.Dequeue(cancellationToken);
                    await _queue.DequeueAsync(cancellationToken);
                    return user.Id;
                },
                None: () =>
                {
                    return Task.FromResult(default(int));
                });

            return result;
        }
    }
}
