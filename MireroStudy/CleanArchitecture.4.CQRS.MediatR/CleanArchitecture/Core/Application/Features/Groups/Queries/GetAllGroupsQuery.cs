using Application.Mappers;
using Application.Persistences;
using MediatR;
using DtoGroup = Api.Groups.Group;

namespace CleanArchitecture.Core.Application.Features.Groups.Queries;

public record GetAllGroupsQuery : IRequest<IEnumerable<DtoGroup>>;

public class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, IEnumerable<DtoGroup>>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IMapper _mapper;
    public GetAllGroupsQueryHandler(IGroupRepository groupRepository, IMapper mapper)
    {
        _groupRepository = groupRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DtoGroup>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _groupRepository.GetAllAsync(cancellationToken);
        var dtos = _mapper.Map<IEnumerable<DtoGroup>>(entities);

        return dtos;
    }
}
