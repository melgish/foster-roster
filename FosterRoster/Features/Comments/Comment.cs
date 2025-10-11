namespace FosterRoster.Features.Comments;

using Felines;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
///     Database entity representing a single journal entry.
/// </summary>
public sealed class Comment : IIdBearer
{
    /// <summary>
    ///     Feline the comment is associated with.
    /// </summary>
    public Feline Feline { get; init; } = null!;

    /// <summary>
    ///     Foreign key for the feline the comment is associated with.
    /// </summary>
    public int FelineId { get; init; }

    /// <summary>
    ///     Unique identifier for the comment.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    ///     If comment was edited, indicates the time of edit.
    /// </summary>
    public DateTimeOffset? Modified { get; set; }

    /// <summary>
    ///     HTML content of the comment.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    ///     Time comment was added to system.
    /// </summary>
    public DateTimeOffset TimeStamp { get; init; }
}

/// <summary>
///     Database configuration for <see cref="Comment"/> entity.
/// </summary>
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
            .Property(e => e.Modified)
            .HasColumnType("timestamp with time zone")
            .IsRequired(false);

        builder
            .Property(e => e.Text)
            .IsRequired()
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