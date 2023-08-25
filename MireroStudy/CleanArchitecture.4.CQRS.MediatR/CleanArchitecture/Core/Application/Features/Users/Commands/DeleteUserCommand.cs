using Application.Mappers;
using Application.Persistences;
using Domain.Entities;
using MediatR;
using DtoUser = Api.Users.User;
using EntityUser = Domain.Entities.User;
namespace CleanArchitecture.Core.Application.Features.Users.Commands;

public record DeleteUserCommand : IRequest<bool>
{
    public long UserId { get; }
    public DeleteUserCommand(long userId)
        => UserId = userId;
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public DeleteUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken = default)
    {
        var result = await _userRepository.DeleteAsync(request.UserId, cancellationToken);
        return result;
    }
}
