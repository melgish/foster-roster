using FosterRoster.Features.Microchips;

namespace FosterRoster.Features.Felines;

using Chores;
using Comments;
using Fosterers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sources;
using Thumbnails;
using Vaccinations;
using Weights;

/// <summary>
///     Database entity representing a feline.
/// </summary>
public sealed class Feline : IIdBearer
{
    public string? AnimalId { get; set; }
    public string? Breed { get; set; }
    public Category Category { get; set; }
    public ICollection<Chore> Chores { get; init; } = [];
    public string? Color { get; set; }
    public ICollection<Comment> Comments { get; init; } = [];
    public Fosterer? Fosterer { get; init; }
    public int? FostererId { get; set; }
    public Gender Gender { get; set; }
    public int Id { get; init; }
    public DateTimeOffset? InactivatedAtUtc { get; init; }
    public int? IntakeAgeInWeeks { get; set; }
    public DateOnly IntakeDate { get; set; }
    public bool IsInactive { get; init; }
    public Microchip? Microchip { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly? RegistrationDate { get; init; }
    public Source? Source { get; init; }
    public int? SourceId { get; set; }
    public DateOnly? SterilizationDate { get; set; }
    public Thumbnail? Thumbnail { get; set; }
    public ICollection<Vaccination> Vaccinations { get; init; } = [];
    public Weaned Weaned { get; set; }
    public ICollection<Weight> Weights { get; init; } = [];
}

/// <summary>
///     Database configuration for <see cref="Feline" />.
/// </summary>
[UsedImplicitly]
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
            .WithMany()
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
            .HasOne(e => e.Microchip)
            .WithOne(e => e.Feline)
            .HasForeignKey<Microchip>(e => e.FelineId)
            .HasConstraintName("FK_Microchips_Felines_FelineId")
            .OnDelete(DeleteBehavior.Cascade);

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
            .Property(e => e.SterilizationDate)
            .HasColumnType("date")
            .IsRequired(false);

        builder
            .HasOne(e => e.Thumbnail)
            .WithOne()
            .HasForeignKey<Thumbnail>(e => e.FelineId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(e => e.Vaccinations)
            .WithOne(e => e.Feline)
            .HasForeignKey(e => e.FelineId)
            .HasConstraintName("FK_Vaccinations_Felines")
            .IsRequired()
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