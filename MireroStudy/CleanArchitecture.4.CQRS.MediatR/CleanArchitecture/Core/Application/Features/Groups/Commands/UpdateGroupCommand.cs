using Application.Mappers;
using Application.Persistences;
using MediatR;
using DtoGroup = Api.Groups.Group;
using EntityGroup = Domain.Entities.Group;

namespace CleanArchitecture.Core.Application.Features.Groups.Commands;

public record UpdateGroupCommand : IRequest<DtoGroup>
{
    public DtoGroup Group { get; }
    public UpdateGroupCommand(DtoGroup group)
        => Group = group;
}

public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, DtoGroup>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IMapper _mapper;
    public UpdateGroupCommandHandler(IGroupRepository groupRepository, IMapper mapper)
    {
        _groupRepository = groupRepository;
        _mapper = mapper;
    }

    public async Task<DtoGroup> Handle(UpdateGroupCommand request, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<EntityGroup>(request.Group);
        var result = await _groupRepository.UpdateAsync(entity, cancellationToken);
        var dto = _mapper.Map<DtoGroup>(result);
        return dto;
    }
}
