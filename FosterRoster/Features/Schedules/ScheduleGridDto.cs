namespace FosterRoster.Features.Schedules;

public class ScheduleGridDto : IIdBearer
{
    /// <summary>
    ///     Cron schedule that defines how the next occurrence of
    ///     a task is calculated.
    /// </summary>
    public required string Cron { get; init; }

    /// <summary>
    ///     Database ID of the schedule.
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    ///     Human-readable name of the schedule.
    /// </summary>
    public required string Name { get; init; }
}