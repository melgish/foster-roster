namespace FosterRoster.Features.Fosterers;

public sealed class FostererGridDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string? Email { get; init; }
    public required string? Phone { get; init; }
}
