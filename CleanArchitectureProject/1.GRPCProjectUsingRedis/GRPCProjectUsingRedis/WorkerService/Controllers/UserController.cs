using Api.Users;
using Application;
using Grpc.Core;
using LanguageExt;
using DtoUser = Api.Users.User;
using EntityUser = Domain.Entities.User;

namespace WorkerService.Controllers
{
    public class UserController : UsersGrpc.UsersGrpcBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(
            ILogger<UserController> logger,
            IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public override async Task<GetUserReply> GetUsers(GetUserRequest request, ServerCallContext context)
        {
            var entities = await _userService.GetUsers();
            var dtos = ConvertToDto(entities).Somes(); // Some인 경우만 추출

            return await Task.FromResult(new GetUserReply()
            {
                Users = { dtos },
            });
        }

        private IEnumerable<Option<DtoUser>> ConvertToDto(IEnumerable<Option<EntityUser>> users)
        {
            return users.Select(userOption => userOption.Map(user => new DtoUser
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
            }));
        }
    }
}
