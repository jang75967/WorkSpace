using Application.Persistences;
using Domain.Entities;
using LanguageExt;
using MediatR;
using WorkerService.Core.Features.Users.Queries;

namespace WorkerService.Core.Features.Users.Handlers
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, IEnumerable<Option<User>>>
    {
        private readonly IBaseRepository<User> _userRepository;

        public GetUsersHandler(IBaseRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Option<User>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.GetAllAsync(cancellationToken);
        }
    }
}
