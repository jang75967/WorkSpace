using Application;
using Domain.Entities;
using LanguageExt;
using MediatR;
using WorkerService.Core.Commands;

namespace WorkerService.Core.Handlers
{
    public class AddUserHandler : IRequestHandler<AddUserCommand, Option<User>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IQueueService _queueService;

        public AddUserHandler(IUserRepository userRepository, IQueueService queueService)
        {
            _userRepository = userRepository;
            _queueService = queueService;
        }

        public async Task<Option<User>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            // grpc response
            await _userRepository.AddUserAsync(request.User, cancellationToken);

            // redis service
            await _queueService.PushAsync(request.User.Name, cancellationToken);

            return Option<User>.Some(request.User);
        }
    }
}
