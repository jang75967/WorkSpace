using DtoUser = Api.Users.User;

namespace CleanArchitecture.Core.Application;

public interface IUserService
{
    Task<IEnumerable<DtoUser>> GetAllUsers();
}
