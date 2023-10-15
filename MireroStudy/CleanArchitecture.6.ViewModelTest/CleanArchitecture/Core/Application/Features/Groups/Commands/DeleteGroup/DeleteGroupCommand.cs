using Application.Mappers;
using Application.Persistences;
using MediatR;
using DtoGroup = Api.Groups.Group;

namespace CleanArchitecture.Core.Application.Features.Groups.Commands;

public record DeleteGroupCommand : IRequest<bool>
{
    public long GroupId { get; }
    public DeleteGroupCommand(long groupId)
        => GroupId = groupId;
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
        var result = await _groupRepository.DeleteAsync(request.GroupId, cancellationToken);
        return result;
    }
}
