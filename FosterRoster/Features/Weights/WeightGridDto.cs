namespace FosterRoster.Features.Weights;

public sealed class WeightGridDto
{
    public int FelineId { get; init; }
    public DateTimeOffset DateTime { get; init; }
    public string Name { get; init; } = string.Empty;
    public float Value { get; init; }
    public WeightUnit Units { get; init; }
}
