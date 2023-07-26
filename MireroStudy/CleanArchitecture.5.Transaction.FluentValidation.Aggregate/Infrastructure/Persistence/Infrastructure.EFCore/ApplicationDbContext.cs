using Domain.Entities;
using Domain.Entities.ActivityAggregate;
using Infrastructure.EFCore.EntityConfigurations;
using Infrastructure.EFCore.EntityInitialize;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Infrastructure.EFCore;

public class ApplicationDbContext : DbContext
{
    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<Group> Groups { get; set; } = null!;
    public virtual DbSet<MemberUserGroup> MemberUserGroups { get; set; } = null!;
    public virtual DbSet<Activity> Activities { get; set; } = null!;
    public virtual DbSet<Expense> Expenses { get; set; } = null!;
    public virtual DbSet<Attendant> Attendees { get; set; } = null!;

    private IDbContextTransaction? _currentTransaction = default!;
    public IDbContextTransaction? GetCurrentTransaction() => _currentTransaction;
    public bool HasActiveTransaction => _currentTransaction != null;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("DM80")
               .ApplyConfiguration(new UserEntityConfiguration())
               .ApplyConfiguration(new GroupEntityConfiguration())
               .ApplyConfiguration(new MemberUserGroupEntityConfiguration())
               .ApplyConfiguration(new ActivityEntityConfiguration())
               .ApplyConfiguration(new AttendantEntityConfiguration())
               .ApplyConfiguration(new ExpenseEntityConfiguration());

        builder.HasUsers()
               .HasGroups()
               .HasMemberUserGroups()
               .HasActivity()
               .HasExpense()
               .HasAttendant();
    }

    public async Task<IDbContextTransaction?> BeginTransactionAsync()
    {
        if (_currentTransaction != null) return null;

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await SaveChangesAsync();
            transaction.Commit();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}
