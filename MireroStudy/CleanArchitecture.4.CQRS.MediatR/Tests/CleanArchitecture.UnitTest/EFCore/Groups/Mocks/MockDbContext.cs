using Domain.Entities;
using Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace CleanArchitecture.UnitTest.EFCore.Groups.Mocks;

public class MockDbContext
{
    private static List<User> UserSeed = GetUserSeed();
    private static List<Group> GroupSeed = GetGroupSeed();
    private static List<MemberUserGroup> MemberUserGroupSeed = GetMemberUserGroupSeed();

    public static Mock<ApplicationDbContext> Get()
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

    private static DbSet<User> UserDbSet()
    {
        var userDbSet = UserSeed.AsQueryable().BuildMockDbSet();

        userDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>(), default))
           .Returns((object[] r, CancellationToken c) =>
           {
               return new ValueTask<User?>(UserSeed.ToList().Find(x => x.Id == (long)r[0]));
           });

        userDbSet.Setup(set => set.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
          .Callback((User entity, CancellationToken _) => UserSeed.Add(entity));

        userDbSet.Setup(set => set.Remove(It.IsAny<User>()))
          .Callback((User entity) =>
          {
              UserSeed.Remove(entity);
              MemberUserGroupSeed.RemoveAll(m => m.UserId == entity.Id);
          });

        return userDbSet.Object;
    }

    private static DbSet<Group> GroupDbSet()
    {
        var groupDbSet = GroupSeed.AsQueryable().BuildMockDbSet();

        groupDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>(), default))
            .Returns((object[] r, CancellationToken c) =>
            {
                return new ValueTask<Group?>(GroupSeed.ToList().Find(x => x.Id == (long)r[0]));
            });

        groupDbSet.Setup(set => set.AddAsync(It.IsAny<Group>(), It.IsAny<CancellationToken>()))
          .Callback((Group entity, CancellationToken _) => GroupSeed.Add(entity));

        groupDbSet.Setup(set => set.Remove(It.IsAny<Group>()))
          .Callback((Group entity) =>
          {
              GroupSeed.Remove(entity);
              MemberUserGroupSeed.RemoveAll(m => m.GroupId == entity.Id);
          });

        return groupDbSet.Object;
    }

    private static DbSet<MemberUserGroup> MemberUserGroupDbSet()
    {
        var membersDbSet = MemberUserGroupSeed.AsQueryable().BuildMockDbSet();

        membersDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>(), default))
            .Returns((object[] r, CancellationToken c) =>
            {
                return new ValueTask<MemberUserGroup?>(MemberUserGroupSeed.ToList().Find(x => x.UserId == (int)r[0] && x.GroupId == (int)r[2]));
            });

        membersDbSet.Setup(set => set.AddAsync(It.IsAny<MemberUserGroup>(), It.IsAny<CancellationToken>()))
          .Callback((MemberUserGroup entity, CancellationToken _) => MemberUserGroupSeed.Add(entity));
        return membersDbSet.Object;
    }

    public static List<User> GetUserSeed()
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

    public static List<Group> GetGroupSeed()
    {
        return new List<Group>()
        {
            new Group() { Id = 1, Name = "축구 동아리" },
            new Group() { Id = 2, Name = "농구 동아리" }

        };
    }

    public static List<MemberUserGroup> GetMemberUserGroupSeed()
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