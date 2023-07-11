using Api.Users;
using CleanArchitecture.Core.Application;
using Grpc.Core;

namespace CleanArchitecture.Services
{
    public class UserController : UsersGrpc.UsersGrpcBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;
        // Controller가 Service에 의존하고 있다.
        // 나중에 의존하는 Service가 많아지면 관리가 힘들다.
        // => 중재자 패턴으로 해결가능
        public UserController(
            ILogger<UserController> logger,
            IUserService userService,
            IGroupService groupService)
        {
            _logger = logger;
            _userService = userService;
            _groupService = groupService;
        }

        public override async Task<GetUserReply> GetUsers(GetUserRequest request, ServerCallContext context)
        {
            var dtos = await _userService.GetAllUsers();
            return new GetUserReply()
            {
                Users = { dtos },
            };
        }

        public override async Task<GetGroupReply> GetGroups(GetGroupRequest request, ServerCallContext context)
        {
            var dtos = await _groupService.GetAllGroups();
            return new GetGroupReply()
            {
                Groups = { dtos }
            };
        }
    }
}