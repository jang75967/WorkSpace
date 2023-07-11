using Application.Mappers;
using Application.Persistences;
using Domain.Entities;
using MediatR;
using DtoUser = Api.Users.User;
using EntityUser = Domain.Entities.User;
namespace CleanArchitecture.Core.Application.Features.Users.Commands;

public class DeleteUserCommand : IRequest<bool>
{
    public DeleteUserCommand(DtoUser user)
    {
        User = user ?? throw new ArgumentNullException(nameof(user));
    }
    public DtoUser User { get; private set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public DeleteUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<EntityUser>(request.User);
        var result = await _userRepository.DeleteAsync(entity, cancellationToken);
        return result;
    }
}
