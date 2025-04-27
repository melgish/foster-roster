namespace FosterRoster.Features.Schedules;

using Data;

public sealed class Schedule : IIdBearer
{
    /// <summary>
    ///     Cron schedule that defines how the next occurrence of
    ///     a task is calculated.
    /// </summary>
    public string Cron { get; set; } = string.Empty;

    /// <summary>
    ///     ID of the schedule.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    ///     Human-readable name of the schedule.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}