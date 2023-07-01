using Application;
using Grpc.Core;
using EntityUser = Domain.Entities.User;
using DtoUser = Api.Users.User;
using Api.Users;
using Application.Mappers;

namespace CleanArchitecture.Services
{
    public class UserController : UsersGrpc.UsersGrpcBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(
            ILogger<UserController> logger,
            IUserService userService,
            IMapper mapper)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
        }

        public override async Task<GetUserReply> GetUsers(GetUserRequest request, ServerCallContext context)
        {
            var entities = await _userService.GetUsers();
            var dtos = _mapper.Map<IEnumerable <DtoUser>>(entities);

            return await Task.FromResult(new GetUserReply()
            {
                Users = { dtos },
            });
        }
    }
}