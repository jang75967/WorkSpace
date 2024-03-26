using Application;
using Domain.Entities;
using LanguageExt;
using MediatR;
using WorkerService.Core.Features.Users.Commands;

namespace WorkerService.Core.Features.Users.Handlers
{
    public class AddUserHandler : IRequestHandler<AddUserCommand, Option<User>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IQueue _queue;

        public AddUserHandler(IUserRepository userRepository, IQueue queue)
        {
            _userRepository = userRepository;
            _queue = queue;
        }

        public async Task<Option<User>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            // grpc response
            await _userRepository.AddUserAsync(request.User, cancellationToken);

            // redis service
            await _queue.EnqueueAsync(request.User.Name, cancellationToken);

            return Option<User>.Some(request.User);
        }
    }
}
