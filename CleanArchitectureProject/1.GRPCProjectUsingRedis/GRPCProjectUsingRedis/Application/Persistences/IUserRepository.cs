using Domain.Entities;

namespace Application.Persistences
{
    public interface IUserRepository : IBaseRepository<User>
    {
        // User 에 특화된 기능이 있을 경우 추가
    }
}
