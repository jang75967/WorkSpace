using Api.Users;
using Application.Mappers;
using Grpc.Core;
using MediatR;
using WorkerService.Core.Features.Users.Commands;
using WorkerService.Core.Features.Users.Queries;
using DtoUser = Api.Users.User;
using EntityUser = Domain.Entities.User;

namespace WorkerService.Controllers
{
    public class UserController : UsersGrpc.UsersGrpcBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMediator _mediator;
        private readonly ICustomMapper _mapper;

        public UserController(
            ILogger<UserController> logger,
            IMediator mediator,
            ICustomMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        public override async Task<GetUserReply> GetUsers(GetUserRequest request, ServerCallContext context)
        {
            var entities = await _mediator.Send(new GetUsersQuery());
            //var dtos = ConvertToDto(entities).Somes(); // Some인 경우만 추출
            var dtos = _mapper.Map<IEnumerable<DtoUser>>(entities.Somes()); // Some인 경우만 추출

            return await Task.FromResult(new GetUserReply()
            {
                Users = { dtos },
            });
        }

        public override async Task<AddUserReply> AddUser(AddUserRequest request, ServerCallContext context)
        {
            await _mediator.Send(new AddUserCommand(new EntityUser
            {
                Id = request.Id,
                Name = request.Name,
                Email = request.Email,
            }));

            var entities = await _mediator.Send(new GetUsersQuery());
            //var dtos = ConvertToDto(entities).Somes(); // Some인 경우만 추출
            var dtos = _mapper.Map<IEnumerable<DtoUser>>(entities.Somes()); // Some인 경우만 추출

            return await Task.FromResult(new AddUserReply()
            {
                Users = { dtos },
            });
        }

        public override async Task<DeleteUserReply> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            await _mediator.Send(new DeleteUserCommand(request.Id));

            var entities = await _mediator.Send(new GetUsersQuery());
            //var dtos = ConvertToDto(entities).Somes(); // Some인 경우만 추출
            var dtos = _mapper.Map<IEnumerable<DtoUser>>(entities.Somes()); // Some인 경우만 추출

            return await Task.FromResult(new DeleteUserReply()
            {
                Users = { dtos },
            });
        }

        //private IEnumerable<Option<DtoUser>> ConvertToDto(IEnumerable<Option<EntityUser>> users)
        //{
        //    return users.Select(userOption => userOption.Map(user => new DtoUser
        //    {
        //        Id = user.Id,
        //        Name = user.Name,
        //        Email = user.Email,
        //    }));
        //}
    }
}
