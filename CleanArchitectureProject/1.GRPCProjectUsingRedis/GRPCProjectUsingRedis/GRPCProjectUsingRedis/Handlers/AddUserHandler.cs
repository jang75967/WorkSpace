using Application;
using Domain.Entities;
using GrpcServiceUsingRedis.Commands;
using LanguageExt;
using MediatR;

namespace GrpcServiceUsingRedis.Handlers
{
    public class AddUserHandler : IRequestHandler<AddUserCommand, Option<User>>
    {
        private readonly IUserService _usersService;

        public AddUserHandler(IUserService usersService)
        {
            _usersService = usersService;
        }

        public async Task<Option<User>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            await _usersService.AddUser(request.User);

            return Option<User>.Some(request.User);
        }
    }
}
