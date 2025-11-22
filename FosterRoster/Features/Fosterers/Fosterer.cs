namespace FosterRoster.Features.Fosterers;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
///     Database entity representing a fosterer.
/// </summary>
public sealed class Fosterer : IIdBearer
{
    /// <summary>
    ///     Mailing label style address of the Fosterer
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    ///     Gets / Sets the preferred contact method for the Fosterer
    /// </summary>
    public ContactMethod ContactMethod { get; set; } = ContactMethod.Email;

    /// <summary>
    ///     Email address of the Fosterer
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    ///     Unique identifier for the Fosterer
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    ///     Date and time the Fosterer was inactivated
    /// </summary>
    public DateTimeOffset? InactivatedAtUtc { get; init; }

    /// <summary>
    ///     True if the Fosterer is not active.
    /// </summary>
    public bool IsInactive { get; init; }

    /// <summary>
    ///     Gets / Sets teh name of the Fosterer
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Primary contact phone.
    /// </summary>
    public string? Phone { get; set; }
}

/// <summary>
///     Database configuration for the <see cref="Fosterer"/> entity.
/// </summary>
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
