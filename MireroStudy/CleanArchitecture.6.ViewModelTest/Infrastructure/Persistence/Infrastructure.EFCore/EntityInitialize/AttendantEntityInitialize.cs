using Infrastructure.Persistence.Common.EntityDatas;
using Microsoft.EntityFrameworkCore;
using EntityAttendant = Domain.Entities.ActivityAggregate.Attendant;
namespace Infrastructure.EFCore.EntityInitialize;

public static class AttendantEntityInitialize
{
    public static ModelBuilder HasAttendant(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EntityAttendant>().HasData(AttendantEntityDatas.Initialize());
        return modelBuilder;
    }
}

