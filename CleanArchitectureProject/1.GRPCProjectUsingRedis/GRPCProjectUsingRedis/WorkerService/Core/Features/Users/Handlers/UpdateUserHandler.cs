using Application.Persistences;
using Domain.Entities;
using LanguageExt;
using MediatR;
using WorkerService.Core.Features.Users.Commands;

namespace WorkerService.Core.Features.Users.Handlers
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Option<User>>
    {
        private readonly IBaseRepository<User> _userRepository;

        public UpdateUserHandler(IBaseRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Option<User>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            // grpc response
            await _userRepository.UpdateAsync(request.User, cancellationToken);

            return Option<User>.Some(request.User);
        }
    }
}
