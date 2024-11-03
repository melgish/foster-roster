using FosterRoster.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FosterRoster.Data.Configurations;

internal sealed class FostererConfiguration : IEntityTypeConfiguration<Fosterer>
{
    public void Configure(EntityTypeBuilder<Fosterer> builder)
    {
        builder.ToTable("Fosterers");

        builder.HasKey(e => e.Id);

        builder
            .Property(e => e.Address)
            .HasMaxLength(256)
            .IsRequired(false);

        builder
            .Property(e => e.ContactMethod)
            .HasMaxLength(16)
            .HasConversion<string>();

        builder
            .Property(e => e.Email)
            .HasMaxLength(64);

        builder
            .Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        builder
            .Property(e => e.InactivatedAtUtc)
            .IsRequired(false);

        builder
            .Property(e => e.IsInactive)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(e => e.Name)
            .HasMaxLength(64)
            .IsRequired();

        builder
            .Property(e => e.Phone)
            .HasMaxLength(16)
            .IsRequired(false);

        builder.HasQueryFilter(e => !e.IsInactive);

        builder
            .HasIndex(e => e.IsInactive)
            .HasFilter("\"IsInactive\" = false");
    }
}