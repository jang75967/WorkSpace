using Infrastructure.Persistence.Common.EntityDatas;
using Microsoft.EntityFrameworkCore;
using EntityGroup = Domain.Entities.Group;

namespace Infrastructure.EFCore.EntityInitialize;

public static class GroupEntityInitialize
{
    public static ModelBuilder HasGroups(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EntityGroup>().HasData(GroupEntityDatas.Initialize());
        return modelBuilder;
    }
}
