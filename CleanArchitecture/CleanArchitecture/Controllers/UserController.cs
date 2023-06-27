using Application;
using Grpc.Core;
using EntityUser = Domain.Entities.User;
using DtoUser = Api.Users.User;
using Api.Users;

namespace CleanArchitecture.Services
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
            var dtos = ConvertToDto(entities);

            return await Task.FromResult(new GetUserReply()
            {
                Users = { dtos },
            });
        }

        private IEnumerable<DtoUser> ConvertToDto(IEnumerable<EntityUser> users)
        {
            var dtos = users.Select(x => new DtoUser
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
            });
            return dtos;
        }
    }
}