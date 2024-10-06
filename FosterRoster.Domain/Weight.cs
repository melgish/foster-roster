namespace FosterRoster.Domain;

public class Weight
{
    public int FelineId { get; set; }

    public DateTimeOffset DateTime { get; set; }

    public float Value { get; set; }

    public WeightUnit Units { get; set; } = WeightUnit.g;

    public virtual Feline Feline { get; set; } = null!;
}

