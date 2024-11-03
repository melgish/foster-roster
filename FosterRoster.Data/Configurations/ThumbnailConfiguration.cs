using FosterRoster.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FosterRoster.Data.Configurations;

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

