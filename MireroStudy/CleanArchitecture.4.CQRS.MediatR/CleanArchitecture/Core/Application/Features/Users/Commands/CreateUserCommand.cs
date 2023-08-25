﻿using Application.Mappers;
using Application.Persistences;
using Infrastructure.EFCore.Repositories;
using MediatR;
using DtoUser = Api.Users.User;
using EntityUser = Domain.Entities.User;

namespace CleanArchitecture.Core.Application.Features.Users.Commands;

public record CreateUserCommand : IRequest<DtoUser>
{
    public DtoUser User { get; }
    public CreateUserCommand(DtoUser user)
        => User = user;
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, DtoUser>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<DtoUser> Handle(CreateUserCommand request, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<EntityUser>(request.User);
        var result = await _userRepository.CreateAsync(entity, cancellationToken);
        var dto = _mapper.Map<DtoUser>(result);
        return dto;
    }
}
