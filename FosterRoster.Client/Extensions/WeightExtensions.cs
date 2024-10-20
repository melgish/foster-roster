using FosterRoster.Domain;

namespace FosterRoster.Client.Extensions;

public static class WeightExtensions
{
    private const float perKg = 1000.0f;
    private const float perOz = 28.3495f;
    private const float perLb = 453.592f;

    private static float Convert(float value, WeightUnit from, WeightUnit to)
        => from switch
        {
            WeightUnit.g => to switch
            {
                WeightUnit.g => value,
                WeightUnit.kg => value / perKg,
                WeightUnit.oz => value / perOz,
                WeightUnit.lbs => value / perLb,
                _ => throw new InvalidOperationException($"Unknown weight unit: {to}")
            },
            WeightUnit.kg => to switch
            {
                WeightUnit.g => value * perKg,
                WeightUnit.kg => value,
                WeightUnit.oz => value * perKg / perOz,
                WeightUnit.lbs => value * perKg / perLb,
                _ => throw new InvalidOperationException($"Unknown weight unit: {to}")
            },
            WeightUnit.oz => to switch
            {
                WeightUnit.g => value * perOz,
                WeightUnit.kg => value * perOz / perKg,
                WeightUnit.oz => value,
                WeightUnit.lbs => value * perOz / perLb,
                _ => throw new InvalidOperationException($"Unknown weight unit: {to}")
            },
            WeightUnit.lbs => to switch
            {
                WeightUnit.g => value * perLb,
                WeightUnit.kg => value * perLb / perKg,
                WeightUnit.oz => value * perLb / perOz,
                WeightUnit.lbs => value,
                _ => throw new InvalidOperationException($"Unknown weight unit: {to}")
            },
            _ => throw new InvalidOperationException($"Unknown weight unit: {from}")
        };
    private static Weight Copy(this Weight weight, float value, WeightUnit units)
        => new Weight
        {
            FelineId = weight.FelineId,
            DateTime = weight.DateTime,
            Value = value,
            Units = units,
            Feline = weight.Feline
        };

    public static Weight ConvertUnits(this Weight weight, WeightUnit units)
        => weight.Units == units
            ? weight
            : weight.Copy(Convert(weight.Value, weight.Units, units), units);


    public static string Format(this Weight weight, WeightUnit? units = null)
    {
        units ??= weight.Units;
        var value = weight.ConvertUnits(units.Value).Value;
        var format = units switch
        {
            WeightUnit.g => "F0",
            WeightUnit.kg => "F2",
            WeightUnit.oz => "F0",
            WeightUnit.lbs => "F2",
            _ => throw new InvalidOperationException($"Unknown weight unit: {units}")
        };

        return $"{value.ToString(format)} {units}";
    }

    public static float Convert(this Weight weight, WeightUnit to)
        => Convert(weight.Value, weight.Units, to);
}
