namespace FosterRoster.Client.Extensions;

public static class WeightExtensions
{
    private const float PerKg = 1000.0f;
    private const float PerOz = 28.3495f;
    private const float PerLb = 453.592f;

    private static float Convert(float value, WeightUnit from, WeightUnit to)
        => from switch
        {
            WeightUnit.g => to switch
            {
                WeightUnit.g => value,
                WeightUnit.kg => value / PerKg,
                WeightUnit.oz => value / PerOz,
                WeightUnit.lbs => value / PerLb,
                _ => throw new InvalidOperationException($"Unknown weight unit: {to}")
            },
            WeightUnit.kg => to switch
            {
                WeightUnit.g => value * PerKg,
                WeightUnit.kg => value,
                WeightUnit.oz => value * PerKg / PerOz,
                WeightUnit.lbs => value * PerKg / PerLb,
                _ => throw new InvalidOperationException($"Unknown weight unit: {to}")
            },
            WeightUnit.oz => to switch
            {
                WeightUnit.g => value * PerOz,
                WeightUnit.kg => value * PerOz / PerKg,
                WeightUnit.oz => value,
                WeightUnit.lbs => value * PerOz / PerLb,
                _ => throw new InvalidOperationException($"Unknown weight unit: {to}")
            },
            WeightUnit.lbs => to switch
            {
                WeightUnit.g => value * PerLb,
                WeightUnit.kg => value * PerLb / PerKg,
                WeightUnit.oz => value * PerLb / PerOz,
                WeightUnit.lbs => value,
                _ => throw new InvalidOperationException($"Unknown weight unit: {to}")
            },
            _ => throw new InvalidOperationException($"Unknown weight unit: {from}")
        };

    private static Weight Copy(this Weight weight, float value, WeightUnit units)
        => new()
        {
            FelineId = weight.FelineId,
            DateTime = weight.DateTime,
            Value = value,
            Units = units,
            Feline = weight.Feline
        };

    private static Weight ConvertUnits(this Weight weight, WeightUnit units)
        => weight.Units == units
            ? weight
            : weight.Copy(Convert(weight.Value, weight.Units, units), units);


    public static string Format(this Weight weight, WeightUnit? units = null)
    {
        units ??= weight.Units;
        var value = weight.ConvertUnits(units.Value).Value;
        var format = units switch
        {
            WeightUnit.g => "N0",
            WeightUnit.kg => "N2",
            WeightUnit.oz => "N0",
            WeightUnit.lbs => "N2",
            _ => throw new InvalidOperationException($"Unknown weight unit: {units}")
        };

        return $"{value.ToString(format)} {units}";
    }
}