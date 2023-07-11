using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EFCore.EntityConfigurations;

public class MemberUserGroupEntityConfiguration : IEntityTypeConfiguration<MemberUserGroup>
{
    public void Configure(EntityTypeBuilder<MemberUserGroup> builder)
    {
        builder.HasIndex(x => x.Id);
        builder.HasKey(e => new { e.UserId, e.GroupId });
    }
}
