namespace FosterRoster.Features.Chores;

using Felines;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
///     Database entity representing a task to be performed for a feline.
/// </summary>
public sealed class Chore : IIdBearer
{
    /// <summary>
    ///     Description of the task. Description will be added
    ///     to journal entry when task is completed.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     Optional date and time when the chore is due. If null,
    ///     no due date will be assigned.
    /// </summary>
    public DateTimeOffset? DueDate { get; set; }

    /// <summary>
    ///     Feline associated with the chore. If null, the chore is
    ///     considered a template chore that can be cloned for
    ///     any feline.
    /// </summary>
    public int? FelineId { get; set; }

    /// <summary>
    ///     Feline associated with the chore. If null, the chore is
    ///     considered a template chore that can be cloned for
    ///     any feline.
    /// </summary>
    public Feline? Feline { get; init; }

    /// <summary>
    ///     Unique identifier for the chore.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    ///     Name of chore to display to the user.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}

/// <summary>
///     Database configuration for <see cref="Chore"/> entity.
/// </summary>
internal sealed class ChoresConfiguration : IEntityTypeConfiguration<Chore>
{
    public void Configure(EntityTypeBuilder<Chore> builder)
    {
        builder.ToTable("Chores");

        builder
            .HasKey(e => e.Id)
            .HasName("PK_Chores");

        builder.Property(e => e.Description)
            .HasMaxLength(256)
            .IsRequired(false);

        builder.Property(e => e.DueDate)
            .IsRequired(false);

        // Configure the relationship to Feline
        builder
            .HasOne(e => e.Feline)
            .WithMany(e => e.Chores)
            .HasForeignKey(e => e.FelineId)
            .HasConstraintName("FK_Chores_Felines")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(e => e.FelineId)
            .IsRequired(false);

        builder
            .Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        builder
            .Property(e => e.Name)
            .HasMaxLength(64)
            .IsRequired();

        // Apply query filter to exclude tasks for inactive felines
        // but include template tasks (where FelineId is null)
        builder
            .HasQueryFilter(e => e.FelineId == null || !e.Feline!.IsInactive);
    }
}
