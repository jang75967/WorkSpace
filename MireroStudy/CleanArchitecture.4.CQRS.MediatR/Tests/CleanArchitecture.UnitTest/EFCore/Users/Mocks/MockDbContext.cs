using Common;
using Domain.Entities;
using Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace CleanArchitecture.UnitTest.EFCore.Users.Mocks;

public class MockDbContext
{
    private List<User> UserSeed = default!;
    private List<Group> GroupSeed = default!;
    private List<MemberUserGroup> MemberUserGroupSeed = default!;

    public MockDbContext()
    {
        UserSeed = GetUserSeed();
        GroupSeed = GetGroupSeed();
        MemberUserGroupSeed = GetMemberUserGroupSeed();
    }

    public Mock<ApplicationDbContext> Get()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .Options;
        var dbContext = new Mock<ApplicationDbContext>(options);
        dbContext.Setup(_ => _.Users).Returns(UserDbSet());
        dbContext.Setup(_ => _.Groups).Returns(GroupDbSet());
        dbContext.Setup(_ => _.MemberUserGroups).Returns(MemberUserGroupDbSet());
        dbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        return dbContext;
    }

    private DbSet<User> UserDbSet()
    {
        return UserSeed.AsQueryable().BuildMockDbSet()
                .FindAsync(UserSeed)
                .AddAsync(UserSeed)
                .Update(UserSeed)
                .Remove(entity =>
                {
                    UserSeed.Remove(entity);
                    MemberUserGroupSeed.RemoveAll(m => m.UserId == entity.Id);
                })
                .Build();
    }

    private DbSet<Group> GroupDbSet()
    {
        return GroupSeed.AsQueryable().BuildMockDbSet()
                .FindAsync(GroupSeed)
                .AddAsync(GroupSeed)
                .Update(GroupSeed)
                .Remove(entity =>
                {
                    GroupSeed.Remove(entity);
                    MemberUserGroupSeed.RemoveAll(m => m.GroupId == entity.Id);
                })
                .Build();
    }

    private DbSet<MemberUserGroup> MemberUserGroupDbSet()
    {
        return MemberUserGroupSeed.AsQueryable().BuildMockDbSet()
               .FindAsync(MemberUserGroupSeed)
               .AddAsync(MemberUserGroupSeed)
               .Build();
    }

    public List<User> GetUserSeed()
    {
        return new List<User>()
        {
            new User() { Id=1, Name = "박영석", Password = "password", Email ="bak@gmail.com"},
            new User() { Id=2, Name = "이건우", Password = "password", Email ="lee@gmail.com"},
            new User() { Id=3, Name = "조범희", Password = "password", Email ="jo@gmail.com"},
            new User() { Id=4, Name = "안성윤", Password = "password", Email ="an@gmail.com"},
            new User() { Id=5, Name = "장동계", Password = "password", Email ="jang@gmail.com"},

        };
    }

    public List<Group> GetGroupSeed()
    {
        return new List<Group>()
        {
            new Group() { Id = 1, Name = "축구 동아리" },
            new Group() { Id = 2, Name = "농구 동아리" }

        };
    }

    public List<MemberUserGroup> GetMemberUserGroupSeed()
    {
        return new List<MemberUserGroup>()
        {
            new MemberUserGroup() { Id = 1, UserId = 1, GroupId = 1, User = UserSeed.Where(_ => _.Id == 1).First(), Group = GroupSeed.Where(_ => _.Id == 1).First() },
            new MemberUserGroup() { Id = 2, UserId = 1, GroupId = 2, User = UserSeed.Where(_ => _.Id == 1).First(), Group = GroupSeed.Where(_ => _.Id == 2).First()},
            new MemberUserGroup() { Id = 3, UserId = 2, GroupId = 1, User = UserSeed.Where(_ => _.Id == 2).First(), Group = GroupSeed.Where(_ => _.Id == 1).First()},
            new MemberUserGroup() { Id = 4, UserId = 3, GroupId = 2, User = UserSeed.Where(_ => _.Id == 3).First(), Group = GroupSeed.Where(_ => _.Id == 2).First()},
            new MemberUserGroup() { Id = 5, UserId = 4, GroupId = 1, User = UserSeed.Where(_ => _.Id == 5).First(), Group = GroupSeed.Where(_ => _.Id == 1).First()},
        };
    }
}
