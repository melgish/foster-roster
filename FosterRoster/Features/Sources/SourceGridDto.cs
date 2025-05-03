namespace FosterRoster.Features.Sources;

public sealed class SourceGridDto : IIdBearer
{
    /// <summary>
    ///     Unique identifier for the source.
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    ///     Name for the source.
    /// </summary>
    public required string Name { get; init; }
}