using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Infra.Configurations;

public abstract class ConfigurationBase<T>
    where T : EntityBase
{
    protected void ConfigureEntityBase(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(k => k.Id);

        builder.HasAlternateKey(x => x.AlternateId);
        builder.HasIndex(x => x.AlternateId).IsUnique();
        builder.Property(x => x.AlternateId).ValueGeneratedOnAdd().IsRequired();

        builder.Property(k => k.Id).ValueGeneratedOnAdd().IsRequired();

        builder.Property(k => k.CreatedUtc).ValueGeneratedOnAdd();

        builder.Property(k => k.DeactivatedUtc);
    }
}
