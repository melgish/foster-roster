using FosterRoster.Features.Felines;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FosterRoster.Features.Weights;

/// <summary>
///     A database entity representing a recorded weight for a feline.
/// </summary>
public sealed class Weight
{
    public DateTimeOffset DateTime { get; init; }
    public Feline Feline { get; init; } = null!;
    public int FelineId { get; init; }
    public WeightUnit Units { get; init; } = WeightUnit.g;
    public float Value { get; init; }
}

/// <summary>
///     Database configuration for the <see cref="Weight" /> entity.
/// </summary>
internal class WeightConfiguration : IEntityTypeConfiguration<Weight>
{
    public void Configure(EntityTypeBuilder<Weight> builder)
    {
        builder.ToTable("Weights");

        builder
            .HasKey(e => new { e.FelineId, e.DateTime })
            .HasName("PK_Weights");

        builder
            .Property(e => e.FelineId)
            .IsRequired();

        builder
            .Property(e => e.DateTime)
            .IsRequired();

        builder
            .Property(e => e.Value)
            .HasColumnType("float")
            .IsRequired();

        builder
            .Property(e => e.Units)
            .HasConversion<string>()
            .IsRequired();

        builder.HasQueryFilter(e => !e.Feline.IsInactive);
    }
}
