namespace FosterRoster.Features.Chores;

public sealed class ChoreCompletionFormDto
{
    /// <summary>
    /// Date and time when the chore was completed.
    /// </summary>
    public DateTimeOffset? LogDate { get; set; }

    /// <summary>
    /// Journal entry for the completed chore.
    /// </summary>
    public string LogText { get; set; } = string.Empty;
}