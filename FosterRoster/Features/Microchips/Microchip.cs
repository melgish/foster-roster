using FosterRoster.Features.Felines;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FosterRoster.Features.Microchips;

public sealed class Microchip: IIdBearer
{
    /// <summary>
    ///     Brand of the microchip.
    /// </summary>
    public string Brand { get; set; } = string.Empty;

    /// <summary>
    ///     Extra comments about the microchip.
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    ///     Feline the chip is associated with.
    /// </summary>
    public Feline Feline { get; init; } = null!;
    
    /// <summary>
    ///     Foreign key for the feline the chip is associated with.
    /// </summary>
    public int FelineId { get; set; }
    
    /// <summary>
    ///     Unique identifier for the relation.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    ///     The chip code / number.
    /// </summary>
    public string MicrochipId { get; set; } = string.Empty;
}

/// <summary>
///     Database configuration for <see cref="Microchip"/> entity.
/// </summary>
internal sealed class MicrochipConfig : IEntityTypeConfiguration<Microchip>
{
    public void Configure(EntityTypeBuilder<Microchip> builder)
    {
        builder.ToTable("Microchips");

        builder.HasKey(e => e.Id);

        builder.Property(m => m.Brand)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(m => m.Comment)
            .HasMaxLength(256)
            .IsRequired(false);

        builder
            .Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        builder.Property(m => m.MicrochipId)
            .HasMaxLength(24)
            .IsRequired();
    }
}