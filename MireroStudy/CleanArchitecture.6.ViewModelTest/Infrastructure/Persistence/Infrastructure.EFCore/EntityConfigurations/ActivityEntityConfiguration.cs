using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EntityActivity = Domain.Entities.ActivityAggregate.Activity;

namespace Infrastructure.EFCore.EntityConfigurations;

public class ActivityEntityConfiguration : IEntityTypeConfiguration<EntityActivity>
{
    public void Configure(EntityTypeBuilder<EntityActivity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
    }
}