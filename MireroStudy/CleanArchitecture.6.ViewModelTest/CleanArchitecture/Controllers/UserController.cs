using Api.Users;
using CleanArchitecture.Core.Application.Features.Users.Commands;
using CleanArchitecture.Core.Application.Features.Users.Queries;
using Grpc.Core;
using MediatR;

namespace CleanArchitecture.Services
{
    public class UserController : UsersGrpc.UsersGrpcBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMediator _mediator;
 
        public UserController(
            ILogger<UserController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public override async Task<GetAllUsersReply> GetAllUsers(GetAllUsersRequest request, ServerCallContext context)
            => new GetAllUsersReply() { Users = { await _mediator.Send(new GetAllUsersQuery()) } };

        public override async Task<GetUserByIdReply> GetUserById(GetUserByIdRequest request, ServerCallContext context)
            => new GetUserByIdReply() { User =  await _mediator.Send(new GetUserByIdQuery( request.Id)) };

        public override async Task<GetMemebersByGroupIdReply> GetMemebersByGroupId(GetMemebersByGroupIdRequest request, ServerCallContext context)
            => new GetMemebersByGroupIdReply() { Users = { await _mediator.Send(new GetAllUsersByGroupIdQuery(request.Id)) } };

        public override async Task<CreateUserReply> CreateUser(CreateUserRequest request, ServerCallContext context)
            => new CreateUserReply() { User = await _mediator.Send(new CreateUserCommand( request.User ))};

        public override async Task<DeleteUserReply> DeleteUser(DeleteUserRequest request, ServerCallContext context)
            => new DeleteUserReply() { Result = await _mediator.Send(new DeleteUserCommand(request.UserId)) };

        public override async Task<UpdateUserReply> UpdateUser(UpdateUserRequest request, ServerCallContext context)
            => new UpdateUserReply() { User = await _mediator.Send(new UpdateUserCommand(request.User)) };
    }
}