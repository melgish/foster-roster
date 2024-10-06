namespace FosterRoster.Data;

using FosterRoster.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class FelineConfiguration : IEntityTypeConfiguration<Feline>
{
    public void Configure(EntityTypeBuilder<Feline> builder)
    {
        builder.ToTable("Felines");

        builder.HasKey(e => e.Id);

        builder
            .Property(e => e.Breed)
            .HasMaxLength(48)
            .IsRequired(false);

        builder
            .Property(e => e.Category)
            .HasConversion<string>()
            .HasMaxLength(16);

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
            .IsRequired(true);

        builder
            .Property(e => e.Name)
            .HasMaxLength(64)
            .IsRequired(true);

        builder
            .Property(e => e.RegistrationDate)
            .HasColumnType("date")
            .IsRequired(false);

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
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}