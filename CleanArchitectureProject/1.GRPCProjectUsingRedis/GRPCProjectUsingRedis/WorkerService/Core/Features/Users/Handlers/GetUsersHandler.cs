using Application;
using Domain.Entities;
using LanguageExt;
using MediatR;
using WorkerService.Core.Features.Users.Queries;

namespace WorkerService.Core.Features.Users.Handlers
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, IEnumerable<Option<User>>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Option<User>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUsersAsync(cancellationToken);
        }
    }
}
