using EntityAttendant = Domain.Entities.ActivityAggregate.Attendant;
namespace Infrastructure.Persistence.Common.EntityDatas;

public class AttendantEntityDatas
{
    public static IEnumerable<EntityAttendant> Initialize()
    {
        return new List<EntityAttendant>()
        {
            new EntityAttendant() { Id = 1, ActivityId = 1, UserId = 1 },
            new EntityAttendant() { Id = 2, ActivityId = 1, UserId = 2 },
            new EntityAttendant() { Id = 3, ActivityId = 2, UserId = 1 },
            new EntityAttendant() { Id = 4, ActivityId = 2, UserId = 3 },
            new EntityAttendant() { Id = 5, ActivityId = 2, UserId = 4 },
            new EntityAttendant() { Id = 6, ActivityId = 2, UserId = 5 },
        };
    }
}

