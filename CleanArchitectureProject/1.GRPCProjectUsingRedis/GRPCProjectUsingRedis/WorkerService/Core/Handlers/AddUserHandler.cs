using Application;
using Domain.Entities;
using LanguageExt;
using MediatR;
using WorkerService.Core.Commands;

namespace WorkerService.Core.Handlers
{
    public class AddUserHandler : IRequestHandler<AddUserCommand, Option<User>>
    {
        private readonly IUserRepository _usersService;
        private readonly IQueueService _redisService;

        public AddUserHandler(IUserRepository usersService, IQueueService redisService)
        {
            _usersService = usersService;
            _redisService = redisService;
        }

        public async Task<Option<User>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            // grpc response
            await _usersService.AddUserAsync(request.User);

            // redis service
            await _redisService.Push(request.User.Name);

            return Option<User>.Some(request.User);
        }
    }
}
