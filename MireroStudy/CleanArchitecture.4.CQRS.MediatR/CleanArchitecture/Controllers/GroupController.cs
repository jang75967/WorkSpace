using Api.Groups;
using CleanArchitecture.Core.Application.Features.Groups.Commands;
using CleanArchitecture.Core.Application.Features.Groups.Queries;
using Grpc.Core;
using MediatR;

namespace CleanArchitecture.Controllers;

public class GroupController : GroupsGrpc.GroupsGrpcBase
{
    private readonly ILogger<GroupController> _logger;
    private readonly IMediator _mediator;

    public GroupController(
        ILogger<GroupController> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public override async Task<GetAllGroupsReply> GetAllGroups(GetAllGroupsRequest request, ServerCallContext context)
        => new GetAllGroupsReply() { Groups = { await _mediator.Send(new GetAllGroupsQuery()) } };

    public override async Task<GetGroupByIdReply> GetGroupById(GetGroupByIdRequest request, ServerCallContext context)
        => new GetGroupByIdReply() { Group = await _mediator.Send(new GetGroupByIdQuery(request.Id)) };

    public override async Task<GetGroupsByUserIdReply> GetGroupsByUserId(GetGroupsByUserIdRequest request, ServerCallContext context)
        => new GetGroupsByUserIdReply() { Groups = { await _mediator.Send(new GetGroupsByUserIdQuery(request.Id)) } };

    public override async Task<CreateGroupReply> CreateGroup(CreateGroupRequest request, ServerCallContext context)
    => new CreateGroupReply() { Group = await _mediator.Send(new CreateGroupCommand(request.Group)) };

    public override async Task<DeleteGroupReply> DeleteGroup(DeleteGroupRequest request, ServerCallContext context)
        => new DeleteGroupReply() { Result = await _mediator.Send(new DeleteGroupCommand(request.GroupId)) };

    public override async Task<UpdateGroupReply> UpdateGroup(UpdateGroupRequest request, ServerCallContext context)
        => new UpdateGroupReply() { Group = await _mediator.Send(new UpdateGroupCommand(request.Group)) };
}
