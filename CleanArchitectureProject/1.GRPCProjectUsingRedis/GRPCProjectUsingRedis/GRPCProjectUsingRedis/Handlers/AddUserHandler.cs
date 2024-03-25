using Application;
using Domain.Entities;
using GrpcServiceUsingRedis.Commands;
using GrpcServiceUsingRedis.Services;
using LanguageExt;
using MediatR;

namespace GrpcServiceUsingRedis.Handlers
{
    public class AddUserHandler : IRequestHandler<AddUserCommand, Option<User>>
    {
        private readonly IUserRepository _usersService;
        private readonly RedisManagerService _redisService;

        public AddUserHandler(IUserRepository usersService, RedisManagerService redisService)
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
