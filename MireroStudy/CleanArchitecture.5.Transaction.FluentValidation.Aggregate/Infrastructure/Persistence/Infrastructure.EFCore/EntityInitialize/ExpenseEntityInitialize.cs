using Infrastructure.Persistence.Common.EntityDatas;
using Microsoft.EntityFrameworkCore;
using EntityExpense = Domain.Entities.ActivityAggregate.Expense;
namespace Infrastructure.EFCore.EntityInitialize;

public static class ExpenseEntityInitialize
{
    public static ModelBuilder HasExpense(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EntityExpense>().HasData(ExpenseEntityDatas.Initialize());
        return modelBuilder;
    }
}

