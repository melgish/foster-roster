namespace FosterRoster.Features.Chores;

using Felines;

public sealed class Chore
{
    /// <summary>
    ///     Description of the task. Description will be added
    ///     to journal entry when task is completed.
    /// </summary>
    public string? Description { get; set; }

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
    ///     How often the chore should be performed. Default is "Once".
    /// </summary>
    public string Frequency { get; init; } = string.Empty;

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