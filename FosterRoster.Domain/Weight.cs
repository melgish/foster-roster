namespace FosterRoster.Domain;

public sealed class Weight
{
    public DateTimeOffset DateTime { get; init; }
    public Feline Feline { get; init; } = null!;
    public int FelineId { get; init; }
    public WeightUnit Units { get; init; } = WeightUnit.g;
    public float Value { get; init; }
}