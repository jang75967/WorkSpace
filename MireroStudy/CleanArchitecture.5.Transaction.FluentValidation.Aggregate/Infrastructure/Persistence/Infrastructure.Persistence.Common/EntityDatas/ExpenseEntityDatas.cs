using EntityExpense = Domain.Entities.ActivityAggregate.Expense;
namespace Infrastructure.Persistence.Common.EntityDatas;

public class ExpenseEntityDatas
{
    public static IEnumerable<EntityExpense> Initialize()
    {
        return new List<EntityExpense>()
        {
            new EntityExpense()
            {
                Id = 1,
                ActivityId = 1,
                Payment = 10000,
            },
            new EntityExpense()
            {
                Id = 2,
                ActivityId = 1,
                Payment = 10000,
            }
            ,new EntityExpense()
            {
                Id = 3,
                ActivityId = 2,
                Payment = 20000,
            },
            new EntityExpense()
            {
                Id = 4,
                ActivityId = 2,
                Payment = 20000,
            }

        };
    }
}
