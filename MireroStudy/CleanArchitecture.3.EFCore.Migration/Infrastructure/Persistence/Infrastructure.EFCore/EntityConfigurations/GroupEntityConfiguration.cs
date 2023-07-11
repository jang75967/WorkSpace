using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EntityGroup = Domain.Entities.Group;

namespace Infrastructure.EFCore.EntityConfigurations;

public class GroupEntityConfiguration : IEntityTypeConfiguration<EntityGroup>
{
    public void Configure(EntityTypeBuilder<EntityGroup> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(30);
    }
}
