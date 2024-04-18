using Domain.Entities;
using InfraStructure.Data.EntityDatas;
using Microsoft.EntityFrameworkCore;

namespace InfraStructure.Data.Persistence.EFCore.EntityInitialize
{
    public static class GroupEntityInitialize
    {
        public static ModelBuilder HasGroups(this ModelBuilder modelBuilder)
        {
            var groups = GroupEntityDatas.Initialize().Somes();
            modelBuilder.Entity<Group>().HasData(groups);
            return modelBuilder;
        }
    }
}
