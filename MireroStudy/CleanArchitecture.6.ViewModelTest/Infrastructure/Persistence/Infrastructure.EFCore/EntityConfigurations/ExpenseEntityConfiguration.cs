using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using EntityActivityExpense = Domain.Entities.ActivityAggregate.Expense;
namespace Infrastructure.EFCore.EntityConfigurations;

internal class ExpenseEntityConfiguration : IEntityTypeConfiguration<EntityActivityExpense>
{
    public void Configure(EntityTypeBuilder<EntityActivityExpense> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ActivityId);
        builder.Property(x => x.Payment);
        builder.HasOne(e => e.Activity).WithMany(e => e.Expenses).HasForeignKey(e => e.ActivityId).IsRequired();
    }
}