using Application.Mappers;
using Application.Persistences;
using MediatR;
using DtoGroup = Api.Groups.Group;
using EntityGroup = Domain.Entities.Group;

namespace CleanArchitecture.Core.Application.Features.Groups.Commands;

public class DeleteGroupCommand : IRequest<bool>
{
    public DeleteGroupCommand(DtoGroup group)
    {
        Group = group;
    }
    public DtoGroup Group { get; private set; }
}

public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, bool>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IMapper _mapper;
    public DeleteGroupCommandHandler(IGroupRepository groupRepository, IMapper mapper)
    {
        _groupRepository = groupRepository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(DeleteGroupCommand request, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<EntityGroup>(request.Group);
        var result = await _groupRepository.DeleteAsync(entity, cancellationToken);
        return result;
    }
}
