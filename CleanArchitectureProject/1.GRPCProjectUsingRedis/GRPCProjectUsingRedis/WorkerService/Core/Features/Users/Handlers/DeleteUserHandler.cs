using Application;
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
            var result = await _userRepository.GetUserByIdAsync(request.Id, cancellationToken).MatchAsync(
                Some: async user =>
                {
                    // grpc response
                    await _userRepository.DeleteUserAsync(user.Id, cancellationToken);

                    // redis service
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
