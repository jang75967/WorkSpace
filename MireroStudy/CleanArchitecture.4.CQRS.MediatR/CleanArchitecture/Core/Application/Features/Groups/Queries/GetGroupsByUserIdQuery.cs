using Application.Mappers;
using Application.Persistences;
using MediatR;
using DtoGroup = Api.Groups.Group;
namespace CleanArchitecture.Core.Application.Features.Groups.Queries;

public record GetGroupsByUserIdQuery : IRequest<IEnumerable<DtoGroup>>
{
    public long UserId { get; set; }
    public GetGroupsByUserIdQuery(long id)
    {
        UserId = id;
    }
}

public class GetGroupsByUserIdQueryHandler : IRequestHandler<GetGroupsByUserIdQuery, IEnumerable<DtoGroup>>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IMapper _mapper;
    public GetGroupsByUserIdQueryHandler(IGroupRepository groupRepository, IMapper mapper)
    {
        _groupRepository = groupRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DtoGroup>> Handle(GetGroupsByUserIdQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _groupRepository.GetGroupsByUserIdAsync(request.UserId, cancellationToken);
        var dtos = _mapper.Map<IEnumerable<DtoGroup>>(entities);
        return dtos;
    }
}