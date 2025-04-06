namespace FosterRoster.Features.Weights;

public sealed class WeightEditModel
{
    public int FelineId { get; set; }
    public float Value { get; set; }
    public DateTimeOffset? DateTime { get; set; }
    public WeightUnit Units { get; set; } = WeightUnit.g;

    public Weight ToWeight() =>
        new()
        {
            FelineId = FelineId,
            Value = Value,
            DateTime = DateTime.GetValueOrDefault().UtcDateTime,
            Units = Units
        };
}