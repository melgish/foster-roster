using FosterRoster.Features.Felines;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FosterRoster.Features.Vaccinations;

/// <summary>
///     Database entity representing a vaccination administered to a feline.
/// </summary>
public sealed class Vaccination : IIdBearer
{
    /// <summary>
    ///     Name of the person or organization that administered the vaccination.
    /// </summary>
    public string AdministeredBy { get; set; } = string.Empty;

    /// <summary>
    ///     Additional comments about the vaccination.
    /// </summary>
    public string? Comments { get; set; }

    /// <summary>
    ///     Date the vaccination expires.
    /// </summary>
    public DateOnly? ExpirationDate { get; set; }

    /// <summary>
    ///     Feline that received the vaccination.
    /// </summary>
    public Feline Feline { get; init; } = null!;

    /// <summary>
    ///     Feline that received the vaccination.
    /// </summary>
    public int FelineId { get; set; }

    /// <summary>
    ///     Name of the vaccine manufacturer.
    /// </summary>
    public string ManufacturerName { get; set; } = string.Empty;

    /// <summary>
    ///     Serial number of the vaccine.
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Date the vaccination was administered.
    /// </summary>
    public DateOnly VaccinationDate { get; set; }

    /// <summary>
    ///     Name of the vaccine administered.
    /// </summary>
    public string VaccineName { get; set; } = string.Empty;

    /// <summary>
    ///     Identifier for the vaccination.
    /// </summary>
    public int Id { get; init; }
}

/// <summary>
///     Database configuration for the <see cref="Vaccination" /> entity.
/// </summary>
internal sealed class VaccinationConfiguration : IEntityTypeConfiguration<Vaccination>
{
    public void Configure(EntityTypeBuilder<Vaccination> builder)
    {
        builder.ToTable("Vaccinations");

        builder
            .HasKey(e => e.Id)
            .HasName("PK_Vaccinations");

        builder.Property(e => e.AdministeredBy)
            .HasMaxLength(64)
            .IsRequired();

        builder
            .Property(e => e.Comments)
            .HasMaxLength(256)
            .IsRequired(false);

        builder
            .Property(e => e.ExpirationDate)
            .HasColumnType("date")
            .IsRequired(false);

        // See <see cref="FelineConfiguration" /> for the relationship
        builder
            .Property(e => e.FelineId)
            .IsRequired();

        builder
            .Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        builder
            .Property(e => e.ManufacturerName)
            .HasMaxLength(64)
            .IsRequired();

        builder
            .Property(e => e.SerialNumber)
            .HasMaxLength(64)
            .IsRequired(false);

        builder
            .Property(e => e.VaccinationDate)
            .HasColumnType("date")
            .IsRequired();

        builder
            .Property(e => e.VaccineName)
            .HasMaxLength(64)
            .IsRequired();

        builder.HasQueryFilter(e => !e.Feline.IsInactive);
    }
}