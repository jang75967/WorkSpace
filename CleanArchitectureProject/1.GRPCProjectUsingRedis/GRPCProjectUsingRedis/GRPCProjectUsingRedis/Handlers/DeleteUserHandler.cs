using Application;
using GrpcServiceUsingRedis.Commands;
using MediatR;

namespace GrpcServiceUsingRedis.Handlers
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, int>
    {
        private readonly IUserRepository _usersService;

        public DeleteUserHandler(IUserRepository usersService)
        {
            _usersService = usersService;
        }

        public async Task<int> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _usersService.GetUserByIdAsync(request.Id).MatchAsync(
                Some: async user =>
                {
                    await _usersService.DeleteUserAsync(user.Id);
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
