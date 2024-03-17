using Application;
using GrpcServiceUsingRedis.Commands;
using MediatR;

namespace GrpcServiceUsingRedis.Handlers
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, int>
    {
        private readonly IUserService _usersService;

        public DeleteUserHandler(IUserService usersService)
        {
            _usersService = usersService;
        }

        public async Task<int> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _usersService.GetUserById(request.Id).MatchAsync(
                Some: async user =>
                {
                    await _usersService.DeleteUser(user.Id);
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
