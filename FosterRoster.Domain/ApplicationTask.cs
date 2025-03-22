namespace FosterRoster.Domain;

public sealed class ApplicationTask
{
    /// <summary>
    ///     Description of the task. Description will be added
    ///     to journal entry when task is completed.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    ///     Feline associated with the task. If null, the task is
    ///     considered a template task that can be cloned for
    ///     any feline.
    /// </summary>
    public int? FelineId { get; init; }

    /// <summary>
    ///     Feline associated with the task. If null, the task is
    ///     considered a template task that can be cloned for
    ///     any feline.
    /// </summary>
    public Feline? Feline { get; init; }

    /// <summary>
    ///     How often the task should be performed. Default is "Once".
    /// </summary>
    public string Frequency { get; init; } = string.Empty;

    /// <summary>
    ///     Unique identifier for the task.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    ///     Name of task to display to the user.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    ///     How many times a task repeats. Default is 1.
    /// </summary>
    public int Repeats { get; init; } = 1;
}