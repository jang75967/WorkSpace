using Infrastructure.Persistence.Common.EntityDatas;
using Microsoft.EntityFrameworkCore;
using EntityActivity = Domain.Entities.ActivityAggregate.Activity;

namespace Infrastructure.EFCore.EntityInitialize;

public static class ActivityEntityInitialize
{
    public static ModelBuilder HasActivity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EntityActivity>().HasData(ActivityEntityDatas.Initialize());
        return modelBuilder;
    }
}

