namespace FosterRoster.Features.Chores;

public sealed class ChoreGridDto
{
    public required string Description { get; init; }
    public required DateTimeOffset? DueDate { get; init; }
    public required string FelineName { get; init; }
    public required int Id { get; init; }
    public required string Name { get; init; }
}
