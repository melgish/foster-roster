namespace FosterRoster.Data.Configurations;

using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class ApplicationTasksConfiguration : IEntityTypeConfiguration<ApplicationTask>
{
    public void Configure(EntityTypeBuilder<ApplicationTask> builder)
    {
        builder.ToTable("ApplicationTasks");

        builder
            .HasKey(e => e.Id)
            .HasName("PK_ApplicationTasks");

        builder.Property(e => e.Description)
            .HasMaxLength(256)
            .IsRequired(false);

        // Configure the relationship to Feline
        builder
            .HasOne(e => e.Feline)
            .WithMany()
            .HasForeignKey(e => e.FelineId)
            .HasConstraintName("FK_ApplicationTasks_Felines")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(e => e.Frequency)
            .HasMaxLength(48)
            .IsRequired()
            .HasDefaultValue("Once");

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