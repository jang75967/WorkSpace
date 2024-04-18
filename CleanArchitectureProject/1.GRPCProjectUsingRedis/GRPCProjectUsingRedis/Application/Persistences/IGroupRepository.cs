using Domain.Entities;

namespace Application.Persistences
{
    public interface IGroupRepository : IBaseRepository<Group>
    {
        // Group 에 특화된 기능이 있을 경우 추가
    }
}
