using Domain.Entities;
using Infrastructure.Persistence.Common.EntityDatas;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EFCore.EntityInitialize;

public static class MemberUserGroupEntityInitialize
{
    public static ModelBuilder HasMemberUserGroups(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MemberUserGroup>().HasData(MemberUserGroupEntityDatas.Initialize());
        return modelBuilder;
    }
}
