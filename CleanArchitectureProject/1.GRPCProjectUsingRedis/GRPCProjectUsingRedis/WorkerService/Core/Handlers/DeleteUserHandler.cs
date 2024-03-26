using Application;
using MediatR;
using WorkerService.Core.Commands;

namespace WorkerService.Core.Handlers
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, int>
    {
        private readonly IUserRepository _userRepository;
        private readonly IQueueService _queueService;

        public DeleteUserHandler(IUserRepository userRepository, IQueueService queueService)
        {
            _userRepository = userRepository;
            _queueService = queueService;
        }

        public async Task<int> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetUserByIdAsync(request.Id).MatchAsync(
                Some: async user =>
                {
                    // grpc response
                    await _userRepository.DeleteUserAsync(user.Id);

                    // redis service
                    await _queueService.PopAsync();
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
