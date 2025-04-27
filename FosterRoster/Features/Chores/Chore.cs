namespace FosterRoster.Features.Chores;

using Felines;

public sealed class Chore
{
    /// <summary>
    ///     Cron expression that defines how the next occurrence of
    ///     a repeating task should be calculated.
    /// </summary>
    public string? Cron { get; set; }

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
    public int? FelineId { get; init; }

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

    /// <summary>
    ///     How many times a chore repeats. Default is 1.
    /// </summary>
    public int Repeats { get; set; } = 1;
}