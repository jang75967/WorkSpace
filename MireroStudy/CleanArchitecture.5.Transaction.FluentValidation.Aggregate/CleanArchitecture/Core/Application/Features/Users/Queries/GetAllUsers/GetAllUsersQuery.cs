using Application.Mappers;
using Application.Persistences;
using MediatR;
using DtoUser = Api.Users.User;

namespace CleanArchitecture.Core.Application.Features.Users.Queries;
public record GetAllUsersQuery : IRequest<IEnumerable<DtoUser>>;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<DtoUser>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public GetAllUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DtoUser>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _userRepository.GetAllAsync(cancellationToken);
        var dtos = _mapper.Map<IEnumerable<DtoUser>>(entities);

        return dtos;
    }
}
