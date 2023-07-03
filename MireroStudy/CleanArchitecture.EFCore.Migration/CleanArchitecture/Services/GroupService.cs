using Application.Mappers;
using Application.Persistences;
using CleanArchitecture.Core.Application;
using DtoGroup = Api.Users.Group;

namespace CleanArchitecture.Services;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;
    private readonly IMapper _mapper;

    public GroupService(IGroupRepository groupRepository, IMapper mapper)
    {
        _groupRepository = groupRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<DtoGroup>> GetAllGroups()
    {
        var entities = await _groupRepository.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<DtoGroup>>(entities);
        return dtos;
    }
}
