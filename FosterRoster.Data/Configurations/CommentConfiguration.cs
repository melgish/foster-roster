namespace FosterRoster.Data.Configurations;

using FosterRoster.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");

        builder.HasKey(e => e.Id);

        builder
            .Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        builder
            .Property(e => e.Text)
            .IsRequired(true)
            .HasColumnType("text")
            .HasMaxLength(4096)
            .HasConversion(new SanitizingValueConverter());

        // Intentionally not using ValueGeneratedOnAdd variants because
        // Postgres 17 INSERT is not working unless TimeStamp is included
        // and set to a value.
        builder
            .Property(e => e.TimeStamp)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()");

        builder
            .HasIndex(e => new { e.FelineId, e.TimeStamp })
            .HasDatabaseName("IX_Comments_FelineId_TimeStamp")
            .IsDescending(false, true);
    }
}

