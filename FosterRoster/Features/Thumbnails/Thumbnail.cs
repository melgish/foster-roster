namespace FosterRoster.Features.Thumbnails;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
///     A database entity for storing thumbnail images associated with felines.
/// </summary>
public sealed class Thumbnail
{
    /// <summary>
    ///     The ID of the feline this thumbnail is associated with.
    /// </summary>
    public int FelineId { get; init; }

    /// <summary>
    ///     The image data for the thumbnail.
    /// </summary>
    public byte[] ImageData { get; set; } = [];

    /// <summary>
    ///     The version of the thumbnail, used for cache busting.
    /// </summary>
    public uint Version { get; init; }

    /// <summary>
    ///     The content type of the image data. Will be image/png unless something
    ///     terrible happens.
    /// </summary>
    public string ContentType { get; set; } = string.Empty;
}

/// <summary>
///     Database configuration for the <see cref="Thumbnail"/> entity.
/// </summary>
internal class ThumbnailConfiguration : IEntityTypeConfiguration<Thumbnail>
{
    public void Configure(EntityTypeBuilder<Thumbnail> builder)
    {
        builder.ToTable("Thumbnails");
        builder.HasKey(e => e.FelineId);

        builder
            .Property(e => e.ContentType)
            .HasMaxLength(128)
            .IsRequired();

        builder
            .Property(e => e.ImageData)
            .HasColumnType("bytea")
            .IsRequired();

        builder
            .Property(e => e.Version)
            .HasColumnName("xmin")
            .HasColumnType("xid")
            .ValueGeneratedOnAddOrUpdate();
    }
}