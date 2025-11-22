namespace FosterRoster.Features.Felines;

public sealed class FelineGridDto
{
    public required string AnimalId { get; init; }
    public required string FostererName { get; init; }
    public int Id { get; init; }
    public bool IsInactive { get; init; }
    public required string Name { get; init; }
}
