using Application;
using Domain.Entities;
using LanguageExt;
using MediatR;
using WorkerService.Commands;
using WorkerService.Services;

namespace WorkerService.Handlers
{
    public class AddUserHandler : IRequestHandler<AddUserCommand, Option<User>>
    {
        private readonly IUserService _usersService;
        private readonly RedisManagerService _redisManagerService;

        public AddUserHandler(IUserService usersService, RedisManagerService redisManagerService)
        {
            _usersService = usersService;
            _redisManagerService = redisManagerService;
        }

        public async Task<Option<User>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            // grpc response
            await _usersService.AddUser(request.User);

            // redis service
            await _redisManagerService.Push(request.User.Name);

            return Option<User>.Some(request.User);
        }
    }
}
