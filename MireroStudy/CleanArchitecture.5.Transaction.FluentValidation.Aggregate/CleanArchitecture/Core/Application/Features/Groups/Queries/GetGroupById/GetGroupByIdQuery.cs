using Application.Mappers;
using Application.Persistences;
using MediatR;
using DtoGroup = Api.Groups.Group;
namespace CleanArchitecture.Core.Application.Features.Groups.Queries;

public record GetGroupByIdQuery : IRequest<DtoGroup>
{
    public long GroupId { get; }
    public GetGroupByIdQuery(long id)
        => GroupId = id;
}

public class GetGroupByIdQueryHandler : IRequestHandler<GetGroupByIdQuery, DtoGroup>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IMapper _mapper;
    public GetGroupByIdQueryHandler(IGroupRepository groupRepository, IMapper mapper)
    {
        _groupRepository = groupRepository;
        _mapper = mapper;
    }

    public async Task<DtoGroup> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken = default)
    {
        var entity = await _groupRepository.GetAsync(request.GroupId, cancellationToken);
        var dto = _mapper.Map<DtoGroup>(entity);
        return dto;
    }
}