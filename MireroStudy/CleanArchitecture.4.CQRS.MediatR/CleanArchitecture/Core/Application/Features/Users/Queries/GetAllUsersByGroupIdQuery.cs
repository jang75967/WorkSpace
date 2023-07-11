using Application.Mappers;
using Application.Persistences;
using MediatR;
using DtoUser = Api.Users.User;
namespace CleanArchitecture.Core.Application.Features.Users.Queries;

public record GetAllUsersByGroupIdQuery : IRequest<IEnumerable<DtoUser>>
{
    public GetAllUsersByGroupIdQuery(long groupId)
    {
        GroupId = groupId;
    }
    public long GroupId { get; private set; }
}

public class GetAllUsersByGroupIdQueryHandler : IRequestHandler<GetAllUsersByGroupIdQuery, IEnumerable<DtoUser>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public GetAllUsersByGroupIdQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DtoUser>> Handle(GetAllUsersByGroupIdQuery request, CancellationToken cancellationToken=default)
    {
        var entities = await _userRepository.GetGroupMembers(request.GroupId, cancellationToken);
        var dtos = _mapper.Map<IEnumerable<DtoUser>>(entities);

        return dtos;
    }
}
