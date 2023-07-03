using Application.Mappers;
using Application.Persistences;
using CleanArchitecture.Core.Application;
using DtoUser = Api.Users.User;

namespace CleanArchitecture.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<DtoUser>> GetAllUsers()
    {
        var entities = await _userRepository.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<DtoUser>>(entities);
        return dtos;
    }
}
