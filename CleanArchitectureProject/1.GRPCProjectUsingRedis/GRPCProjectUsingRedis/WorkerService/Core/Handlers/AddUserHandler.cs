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
        private readonly IQueueService _redisService;

        public AddUserHandler(IUserRepository userRepository, IQueueService redisService)
        {
            _userRepository = userRepository;
            _redisService = redisService;
        }

        public async Task<Option<User>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            // grpc response
            await _userRepository.AddUserAsync(request.User);

            // redis service
            await _redisService.Push(request.User.Name);

            return Option<User>.Some(request.User);
        }
    }
}
