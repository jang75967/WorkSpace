using Application.Mappers;
using Application.Persistences;
using MediatR;
using DtoUser = Api.Users.User;

namespace CleanArchitecture.Core.Application.Features.Users.Queries;

public record GetUserByIdQuery : IRequest<DtoUser>
{
    public long UserId { get; }
    public GetUserByIdQuery(long id)
        => UserId = id;
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, DtoUser>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<DtoUser> Handle(GetUserByIdQuery request, CancellationToken cancellationToken = default)
    {
        var entity = await _userRepository.GetAsync(request.UserId, cancellationToken);
        var dtos = _mapper.Map<DtoUser>(entity);

        return dtos;
    }
}
