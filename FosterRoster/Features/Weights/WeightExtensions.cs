namespace FosterRoster.Features.Weights;

public static class WeightExtensions
{
    private const float PerKg = 1000.0f;
    private const float PerOz = 28.3495f;
    private const float PerLb = 453.592f;

    private static InvalidOperationException InvalidUnit(WeightUnit unit)
        => new($"Invalid weight unit: {unit}");

    private static float Convert(this float value, WeightUnit from, WeightUnit to)
        => from switch
        {
            WeightUnit.g => to switch
            {
                WeightUnit.g => value,
                WeightUnit.kg => value / PerKg,
                WeightUnit.oz => value / PerOz,
                WeightUnit.lbs => value / PerLb,
                _ => throw InvalidUnit(to)
            },
            WeightUnit.kg => to switch
            {
                WeightUnit.g => value * PerKg,
                WeightUnit.kg => value,
                WeightUnit.oz => value * PerKg / PerOz,
                WeightUnit.lbs => value * PerKg / PerLb,
                _ => throw InvalidUnit(to)
            },
            WeightUnit.oz => to switch
            {
                WeightUnit.g => value * PerOz,
                WeightUnit.kg => value * PerOz / PerKg,
                WeightUnit.oz => value,
                WeightUnit.lbs => value * PerOz / PerLb,
                _ => throw InvalidUnit(to)
            },
            WeightUnit.lbs => to switch
            {
                WeightUnit.g => value * PerLb,
                WeightUnit.kg => value * PerLb / PerKg,
                WeightUnit.oz => value * PerLb / PerOz,
                WeightUnit.lbs => value,
                _ => throw InvalidUnit(to)
            },
            _ => throw InvalidUnit(to)
        };

    public static string Format(this float value, WeightUnit from, WeightUnit to)
    {
        value = value.Convert(from, to);
        var format = to switch
        {
            WeightUnit.g => "N0",
            WeightUnit.kg => "N2",
            WeightUnit.oz => "N0",
            WeightUnit.lbs => "N2",
            _ => throw InvalidUnit(to)
        };
        return $"{value.ToString(format)} {to}";
    }
}