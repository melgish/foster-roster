using FosterRoster.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FosterRoster.Data.Configurations;

internal sealed class FelineConfiguration : IEntityTypeConfiguration<Feline>
{
    public void Configure(EntityTypeBuilder<Feline> builder)
    {
        builder.ToTable("Felines");

        builder.HasKey(e => e.Id);

        builder
            .Property(e => e.AnimalId)
            .HasMaxLength(24)
            .IsRequired(false);

        builder
            .Property(e => e.Breed)
            .HasMaxLength(48)
            .IsRequired(false);

        builder
            .Property(e => e.Category)
            .HasConversion<string>()
            .HasMaxLength(16);

        builder
            .Property(e => e.Color)
            .HasMaxLength(96)
            .IsRequired(false);

        builder
            .HasMany(e => e.Comments)
            .WithOne(e => e.Feline)
            .HasForeignKey(e => e.FelineId)
            .HasConstraintName("FK_Comments_Felines")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Fosterer)
            .WithMany(e => e.Felines)
            .HasForeignKey(e => e.FostererId)
            .HasConstraintName("FK_Fosterers_Felines")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .Property(e => e.Gender)
            .HasConversion<string>()
            .HasMaxLength(16);

        builder
            .Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        builder
            .Property(e => e.IntakeAgeInWeeks)
            .IsRequired(false);

        builder
            .Property(e => e.IntakeDate)
            .HasColumnType("date")
            .IsRequired();

        builder
            .Property(e => e.Name)
            .HasMaxLength(64)
            .IsRequired();

        builder
            .Property(e => e.RegistrationDate)
            .HasColumnType("date")
            .IsRequired(false);

        builder.HasOne(e => e.Source)
            .WithMany()
            .HasForeignKey(e => e.SourceId)
            .HasConstraintName("FK_Sources_Felines")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasOne(e => e.Thumbnail)
            .WithOne()
            .HasForeignKey<Thumbnail>(e => e.FelineId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(e => e.Weaned)
            .HasConversion<string>()
            .HasMaxLength(16);

        builder
            .HasMany(e => e.Weights)
            .WithOne(e => e.Feline)
            .HasForeignKey(e => e.FelineId)
            .HasConstraintName("FK_Weights_Felines")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(e => e.IsInactive)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(e => e.InactivatedAtUtc)
            .IsRequired(false);

        builder.HasQueryFilter(e => !e.IsInactive);

        builder
            .HasIndex(e => e.IsInactive)
            .HasFilter("\"IsInactive\" = false");
    }
}