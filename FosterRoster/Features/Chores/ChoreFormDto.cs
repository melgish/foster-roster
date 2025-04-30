namespace FosterRoster.Features.Chores;

public sealed class ChoreFormDto
{
    /// <summary>
    ///     Description of the task. Description will be added
    ///     to journal entry when task is completed.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     Date task is due. If null, no due date will be assigned.
    /// </summary>
    public DateTimeOffset? DueDate { get; set; }

    /// <summary>
    ///     Feline associated with the task. If null, the task is
    ///     considered a template task that can be cloned for
    ///     any feline.
    /// </summary>
    public int FelineId { get; set; }

    /// <summary>
    ///     How often the task should be performed. Default is "Once".
    /// </summary>
    public string? Cron { get; set; }

    /// <summary>
    ///     Unique identifier for the task.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    ///     Name of task to display to the user.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     How many times a task repeats. Default is 1.
    /// </summary>
    public int Repeats { get; set; } = 1;
}