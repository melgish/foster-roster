namespace FosterRoster.Features.Schedules;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class Schedule : IIdBearer
{
    /// <summary>
    ///     Cron schedule that defines how the next occurrence of
    ///     a task is calculated.
    /// </summary>
    public string Cron { get; init; } = string.Empty;

    /// <summary>
    ///     ID of the schedule.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    ///     Human-readable name of the schedule.
    /// </summary>
    public string Name { get; init; } = string.Empty;
}

internal sealed class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.ToTable("Schedules");

        builder
            .HasKey(e => e.Id)
            .HasName("PK_Schedules");

        builder
            .Property(e => e.Cron)
            .HasMaxLength(128)
            .IsRequired();

        builder
            .Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        builder
            .Property(e => e.Name)
            .HasMaxLength(48)
            .IsRequired();

        // Cron must be unique in order to map schedule to a friendly name
        // without a hard relation.
        builder
            .HasIndex(e => e.Cron)
            .IsUnique()
            .HasDatabaseName("IX_Schedules_Cron");
    }
}