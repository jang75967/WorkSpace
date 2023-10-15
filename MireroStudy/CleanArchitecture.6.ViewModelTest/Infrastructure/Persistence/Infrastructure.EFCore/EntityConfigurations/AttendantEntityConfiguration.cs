using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EntityAttendant = Domain.Entities.ActivityAggregate.Attendant;

namespace Infrastructure.EFCore.EntityConfigurations;

public class AttendantEntityConfiguration : IEntityTypeConfiguration<EntityAttendant>
{
    public void Configure(EntityTypeBuilder<EntityAttendant> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ActivityId);
        builder.Property(x => x.UserId);
        
        builder.HasOne(x => x.Activity).WithMany(t => t.Attendees).HasForeignKey(x => x.ActivityId).IsRequired();
        builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).IsRequired();
    }
}