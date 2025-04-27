namespace FosterRoster.Features.Chores;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class ChoresConfiguration : IEntityTypeConfiguration<Chore>
{
    public void Configure(EntityTypeBuilder<Chore> builder)
    {
        builder.ToTable("Chores");

        builder
            .HasKey(e => e.Id)
            .HasName("PK_Chores");

        builder.Property(e => e.Cron)
            .HasMaxLength(128)
            .IsRequired(false);

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

        builder
            .Property(e => e.Repeats)
            .IsRequired()
            .HasDefaultValue(1);

        // Apply query filter to exclude tasks for inactive felines
        // but include template tasks (where FelineId is null)
        builder
            .HasQueryFilter(e => e.FelineId == null || !e.Feline!.IsInactive);
    }
}