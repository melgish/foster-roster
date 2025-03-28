namespace FosterRoster.Domain;

public sealed class Source
{
    /// <summary>
    /// Unique identifier for the source.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Name for the source.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}