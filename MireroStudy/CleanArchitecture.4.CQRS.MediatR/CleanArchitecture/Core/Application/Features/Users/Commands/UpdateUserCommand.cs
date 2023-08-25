using Application.Mappers;
using Application.Persistences;
using MediatR;
using DtoUser = Api.Users.User;
using EntityUser = Domain.Entities.User;
namespace CleanArchitecture.Core.Application.Features.Users.Commands;

public record UpdateUserCommand : IRequest<DtoUser>
{
    public DtoUser User { get; private set; }
    public UpdateUserCommand(DtoUser user)
         => User = user;
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, DtoUser>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<DtoUser> Handle(UpdateUserCommand request, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<EntityUser>(request.User);
        var result = await _userRepository.UpdateAsync(entity, cancellationToken);
        var dto = _mapper.Map<DtoUser>(result);
        return dto;
    }
}
