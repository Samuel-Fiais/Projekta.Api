using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Infra.Configurations;

public class TeamMemberConfiguration
    : ConfigurationBase<TeamMember>,
        IEntityTypeConfiguration<TeamMember>
{
    public void Configure(EntityTypeBuilder<TeamMember> builder)
    {
        ConfigureEntityBase(builder);

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.TeamMembers)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Project)
            .WithMany(x => x.TeamMembers)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
