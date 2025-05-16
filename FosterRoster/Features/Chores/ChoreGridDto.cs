namespace FosterRoster.Features.Chores;

public sealed class ChoreGridDto
{
    public required DateTimeOffset? DueDate { get; init; }
    public string? FelineName { get; init; } = string.Empty;
    public required int Id { get; init; }
    public required string Name { get; init; }
}