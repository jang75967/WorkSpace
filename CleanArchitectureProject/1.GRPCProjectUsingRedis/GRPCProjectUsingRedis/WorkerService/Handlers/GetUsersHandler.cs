using Application;
using Domain.Entities;
using LanguageExt;
using MediatR;
using WorkerService.Queries;

namespace WorkerService.Handlers
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, IEnumerable<Option<User>>>
    {
        private readonly IUserService _userService;

        public GetUsersHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IEnumerable<Option<User>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetUsers();
        }
    }
}
