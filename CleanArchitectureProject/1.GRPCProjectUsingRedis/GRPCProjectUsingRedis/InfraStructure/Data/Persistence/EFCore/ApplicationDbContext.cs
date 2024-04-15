using Domain.Entities;
using InfraStructure.Data.Persistence.EFCore.EntityConfigurations;
using InfraStructure.Data.Persistence.EFCore.EntityInitialize;
using Microsoft.EntityFrameworkCore;

namespace InfraStructure.Data.Persistence.EFCore
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Group> Groups { get; set; } = default!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("JDGPostgresEFCore")
                   .ApplyConfiguration(new UserEntityConfiguration())
                   .ApplyConfiguration(new GroupEntityConfiguration());

            builder.HasUsers()
                   .HasGroups();
        }
    }
}
