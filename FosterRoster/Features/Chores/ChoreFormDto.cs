namespace FosterRoster.Features.Chores;

using Data;

public sealed class ChoreFormDto()
{
    public ChoreFormDto(Chore task) : this()
    {
        Cron = task.Cron;
        Description = task.Description;
        DueDate = task.DueDate;
        FelineId = task.FelineId.GetValueOrDefault();
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
    public int Id { get; set; }

    /// <summary>
    ///     Name of task to display to the user.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     How many times a task repeats. Default is 1.
    /// </summary>
    public int Repeats { get; set; } = 1;

    public Chore ToChore() =>
        new()
        {
            Cron = Cron.TrimToNull(),
            Description = Description.TrimToNull(),
            DueDate = DueDate?.UtcDateTime,
            FelineId = FelineId == 0 ? null : FelineId,
            Id = Id,
            Name = Name.TrimToNull(),
            Repeats = Repeats
        };
}