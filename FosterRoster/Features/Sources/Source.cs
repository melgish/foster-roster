namespace FosterRoster.Features.Sources;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
///     A database entity representing the source of a fostered animal.
/// </summary>
public sealed class Source : IIdBearer
{
    /// <summary>
    ///     Unique identifier for the source.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    ///     Name for the source.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}

/// <summary>
///     Database configuration for the <see cref="Source"/> entity.
/// </summary>
internal sealed class SourceConfiguration : IEntityTypeConfiguration<Source>
{
    public void Configure(EntityTypeBuilder<Source> builder)
    {
        builder.ToTable("Sources");

        builder.HasKey(e => e.Id);

        builder
            .Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        builder
            .Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(64);
    }
}
