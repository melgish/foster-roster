namespace FosterRoster.Domain;

public sealed class ApplicationTaskEditModel()
{
    public ApplicationTaskEditModel(ApplicationTask task) : this()
    {
        Description = task.Description;
        FelineId = task.FelineId.GetValueOrDefault();
        Frequency = task.Frequency;
        Id = task.Id;
        Name = task.Name;
        Repeats = task.Repeats;
    }

    /// <summary>
    ///     Description of the task. Description will be added
    ///     to journal entry when task is completed.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     Feline associated with the task. If null, the task is
    ///     considered a template task that can be cloned for
    ///     any feline.
    /// </summary>
    public int FelineId { get; set; }

    /// <summary>
    ///     How often the task should be performed. Default is "Once".
    /// </summary>
    public string Frequency { get; set; } = "Once";

    /// <summary>
    ///     Unique identifier for the task.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Name of task to display to the user.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     How many times a task repeats. Default is 1.
    /// </summary>
    public int Repeats { get; set; } = 1;

    public ApplicationTask ToApplicationTask() =>
        new()
        {
            Description = Description,
            FelineId = FelineId == 0 ? null : FelineId,
            Frequency = Frequency,
            Id = Id,
            Name = Name,
            Repeats = Repeats
        };
}