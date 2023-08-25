using DtoGroup = Api.Users.Group;
namespace CleanArchitecture.Core.Application;

public interface IGroupService
{
    Task<IEnumerable<DtoGroup>> GetAllGroups(CancellationToken cancellationToken = default);
}
