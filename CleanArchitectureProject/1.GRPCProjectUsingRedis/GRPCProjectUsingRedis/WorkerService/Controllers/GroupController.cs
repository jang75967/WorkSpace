using Api.Groups;
using Application.Mappers;
using Grpc.Core;
using MediatR;
using WorkerService.Core.Features.Groups.Commands;
using WorkerService.Core.Features.Groups.Queries;
using DtoGroup = Api.Groups.Group;
using EntityGroup = Domain.Entities.Group;

namespace WorkerService.Controllers
{
    public class GroupController : GroupsGrpc.GroupsGrpcBase
    {
        private readonly ILogger<GroupController> _logger;
        private readonly IMediator _mediator;
        private readonly ICustomMapper _mapper;

        public GroupController(ILogger<GroupController> logger, IMediator mediator, ICustomMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        public override async Task<GetGroupReply> GetGroups(GetGroupRequest request, ServerCallContext context)
        {
            var entities = await _mediator.Send(new GetGroupsQuery());
            var dtos = _mapper.Map<IEnumerable<DtoGroup>>(entities.Somes());

            return await Task.FromResult(new GetGroupReply()
            {
                Groups = {dtos },
            });
        }

        public override async Task<AddGroupReply> AddGroup(AddGroupRequest request, ServerCallContext context)
        {
            await _mediator.Send(new AddGroupCommand(new EntityGroup
            {
                Id = request.Id,
                Name = request.Name,
            }));

            var entities = await _mediator.Send(new GetGroupsQuery()); // 목록 최신화 위해서 테스트용 호출 (보통 필요없음)
            var dtos = _mapper.Map<IEnumerable<DtoGroup>>(entities.Somes());

            return await Task.FromResult(new AddGroupReply()
            {
                Groups = { dtos },
            });
        }

        public override async Task<UpdateGroupReply> UpdateGroup(UpdateGroupRequest request, ServerCallContext context)
        {
            await _mediator.Send(new UpdateGroupCommand(new EntityGroup
            {
                Id = request.Id,
                Name = request.Name,
            }));

            var entities = await _mediator.Send(new GetGroupsQuery());
            var dtos = _mapper.Map<IEnumerable<DtoGroup>>(entities.Somes());

            return await Task.FromResult(new UpdateGroupReply()
            {
                Groups = { dtos },
            });
        }

        public override async Task<DeleteGroupReply> DeleteGroup(DeleteGroupRequest request, ServerCallContext context)
        {
            await _mediator.Send(new DeleteGroupCommand(request.Id));

            var entities = await _mediator.Send(new GetGroupsQuery());
            var dtos = _mapper.Map<IEnumerable<DtoGroup>>(entities.Somes());

            return await Task.FromResult(new DeleteGroupReply()
            {
                Groups = { dtos },
            });
        }
    }
}
