using Domain.Entities;
using Infrastructure.Persistence.Common.EntityDatas;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EFCore.EntityInitialize;

public static class UserEntityInitialize
{
    public static ModelBuilder HasUsers(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(UserEntityDatas.Initialize());
        return modelBuilder;
    }
}
