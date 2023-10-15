using Api.Users;
using Application.Mappers;
using Client.Business.Core.Domain.Attributes;
using Client.Business.Core.Domain.Models.Users;
using Grpc.Net.Client;
using LanguageExt;
using MediatR;

namespace Client.Business.Core.Application.Features.Users.Queries;

[RetryPolicy(RetryCount = 2, SleepDuration = 500)]
public record GetAllUsersQuery : IRequest<Option<IEnumerable<UserModel>>>;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Option<IEnumerable<UserModel>>>
{
    private readonly IMapper _mapper;
    private readonly GrpcChannel _channel;
    public GetAllUsersQueryHandler(IMapper mapper, GrpcChannel channel)
    {
        _mapper = mapper;
        _channel = channel;
    }

    public async Task<Option<IEnumerable<UserModel>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken = default)
    {
        var client = new UsersGrpc.UsersGrpcClient(_channel);
        var response = await client.GetAllUsersAsync(new GetAllUsersRequest { });

        var models = _mapper.Map<IEnumerable<UserModel>>(response.Users);
        if (models is null)
            return Option<IEnumerable<UserModel>>.None;

        return Option<IEnumerable<UserModel>>.Some(models);
    }
}
