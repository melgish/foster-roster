namespace FosterRoster.Domain;

public class Weight
{
    public DateTimeOffset DateTime { get; set; }
    public virtual Feline Feline { get; set; } = null!;
    public int FelineId { get; set; }
    public WeightUnit Units { get; set; } = WeightUnit.g;
    public float Value { get; set; }
}

