using EntityActivity = Domain.Entities.ActivityAggregate.Activity;
namespace Infrastructure.Persistence.Common.EntityDatas;

public class ActivityEntityDatas
{
    public static IEnumerable<EntityActivity> Initialize()
    {
        return new List<EntityActivity>()
        {
            new EntityActivity() 
            { 
                Id = 1, 
                GroupId= 1,
                Title="체육대회",
                TotalPayment = 20000
            },
            new EntityActivity()
            {
                Id = 2,
                GroupId= 2,
                Title="체육대회",
                TotalPayment = 40000
            }
        };
    }
}
