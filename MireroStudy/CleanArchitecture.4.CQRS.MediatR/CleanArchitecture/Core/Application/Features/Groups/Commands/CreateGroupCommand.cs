using Application.Mappers;
using Application.Persistences;
using MediatR;
using DtoGroup = Api.Groups.Group;
using EntityGroup = Domain.Entities.Group;
namespace CleanArchitecture.Core.Application.Features.Groups.Commands;

public class CreateGroupCommand : IRequest<DtoGroup>
{
    public CreateGroupCommand(DtoGroup group)
    {
        Group = group;
    }
    public DtoGroup Group { get; private set; }
}

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, DtoGroup>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IMapper _mapper;
    public CreateGroupCommandHandler(IGroupRepository groupRepository, IMapper mapper)
    {
        _groupRepository = groupRepository;
        _mapper = mapper;
    }

    public async Task<DtoGroup> Handle(CreateGroupCommand request, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<EntityGroup>(request.Group);
        var result = await _groupRepository.CreateAsync(entity, cancellationToken);
        var dto = _mapper.Map<DtoGroup>(result);
        return dto;
    }
}
