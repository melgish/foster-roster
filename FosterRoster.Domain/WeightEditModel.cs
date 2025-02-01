namespace FosterRoster.Domain;

public sealed class WeightEditModel()
{
    public int FelineId { get; set; }
    public float Value { get; set; }
    public DateTimeOffset? DateTime { get; set; }
    public WeightUnit Units { get; set; } = WeightUnit.g;

    public WeightEditModel(Weight weight) : this()
    {
        FelineId = weight.FelineId;
        Value = weight.Value;
        DateTime = weight.DateTime.UtcDateTime;
        Units = weight.Units;
    }

    public Weight ToWeight() =>
        new()
        {
            FelineId = FelineId,
            Value = Value,
            DateTime = DateTime.GetValueOrDefault().UtcDateTime,
            Units = Units
        };
}