using Common;
using Domain.Entities.ActivityAggregate;
using Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using MockQueryable.Moq;
using Moq;

namespace CleanArchitecture.UnitTest.EFCore.Activities.Mocks;

public class MockDbContext
{
    private List<Activity> ActivitySeed = default!;
    private List<Expense> ExpenseSeed = default!;
    private List<Attendant> AttendantSeed = default!;

    public MockDbContext()
    {
        ActivitySeed = GetActivitySeed();
        ExpenseSeed = ActivitySeed.SelectMany(x => x.Expenses).ToList();
        AttendantSeed = ActivitySeed.SelectMany(x => x.Attendees).ToList();
    }

    public Mock<ApplicationDbContext> Get()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .Options;
        var dbContext = new Mock<ApplicationDbContext>(options);
        dbContext.Setup(_ => _.Activities).Returns(ActivityDbSet);
        dbContext.Setup(_ => _.Expenses).Returns(ExpenseDbSet);
        dbContext.Setup(_ => _.Attendees).Returns(AttendantDbSet);
        dbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        return dbContext;
    }

    private DbSet<Activity> ActivityDbSet()
    {
        return ActivitySeed.AsQueryable().BuildMockDbSet()
                .FindAsync(ActivitySeed)
                .AddAsync(ActivitySeed)
                .Update(ActivitySeed)
                .Remove(entity =>
                {
                    ActivitySeed.Remove(entity);
                    ExpenseSeed.RemoveAll(m => m.ActivityId == entity.Id);
                })
                .Build();
    }

    private DbSet<Expense> ExpenseDbSet() => ExpenseSeed.AsQueryable().BuildMockDbSet().Object;

    private DbSet<Attendant> AttendantDbSet() => AttendantSeed.AsQueryable().BuildMockDbSet().Object;


    public List<Activity> GetActivitySeed()
    {
        return new List<Activity>()
        {
            new Activity()
            {
                Id = 1,
                GroupId= 1,
                Title="체육대회",
                Expenses = GetExpenseSeed().Where(x => x.ActivityId == 1).ToList(),
                Attendees = GetAttendantSeed().Where(x => x.ActivityId == 1).ToList()
            },
            new Activity()
            {
                Id = 2,
                GroupId= 2,
                Title="체육대회",
                Expenses = GetExpenseSeed().Where(x => x.ActivityId == 2).ToList(),
                Attendees = GetAttendantSeed().Where(x => x.ActivityId == 2).ToList()
            }
        };
    }


    public List<Attendant> GetAttendantSeed()
    {
        return new List<Attendant>()
        {
            new Attendant() { Id = 1, ActivityId = 1, UserId = 1 },
            new Attendant() { Id = 2, ActivityId = 1, UserId = 2 },
            new Attendant() { Id = 3, ActivityId = 2, UserId = 1 },
            new Attendant() { Id = 4, ActivityId = 2, UserId = 3 },
            new Attendant() { Id = 5, ActivityId = 2, UserId = 4 },
            new Attendant() { Id = 6, ActivityId = 2, UserId = 5 },
        };
    }

    public List<Expense> GetExpenseSeed()
    {
        return new List<Expense>()
        {
            new Expense()
            {
                Id = 1,
                ActivityId = 1,
                Payment = 10000,
            },
            new Expense()
            {
                Id = 2,
                ActivityId = 1,
                Payment = 10000,
            }
            ,new Expense()
            {
                Id = 3,
                ActivityId = 2,
                Payment = 20000,
            },
            new Expense()
            {
                Id = 4,
                ActivityId = 2,
                Payment = 20000,
            }
        };
    }
}
