using Application;
using Domain.Entities;
using GrpcServiceUsingRedis.Queries;
using LanguageExt;
using MediatR;

namespace GrpcServiceUsingRedis.Handlers
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, IEnumerable<Option<User>>>
    {
        private readonly IUserRepository _userService;

        public GetUsersHandler(IUserRepository userService)
        {
            _userService = userService;
        }

        public async Task<IEnumerable<Option<User>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetUsersAsync();
        }
    }
}
