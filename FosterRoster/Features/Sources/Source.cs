namespace FosterRoster.Features.Sources;

public sealed class Source : IIdBearer
{
    /// <summary>
    ///     Unique identifier for the source.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    ///     Name for the source.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}