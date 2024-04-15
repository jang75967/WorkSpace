using Domain.Entities;
using InfraStructure.Data.EntityDatas;
using Microsoft.EntityFrameworkCore;

namespace InfraStructure.Data.Persistence.EFCore.EntityInitialize
{
    public static class UserEntityInitialize
    {
        public static ModelBuilder HasUsers(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(UserEntityDatas.Initialize());
            return modelBuilder;
        }
    }
}
