namespace FosterRoster.Domain;

using System.Data;

using FluentValidation;

public sealed class WeightEditModel()
{
    public int FelineId { get; set; }
    public float Value { get; set; }

    // This is a computed property that combines the Date and Time properties
    // because MudBlazor does not have a Date+Time picker.
    public DateTime? DateTime {
        get => (Date, Time) switch
            {
                (null, null) => default(DateTime?),
                (var date, null) => date.Value.Date,
                (null, var time) => System.DateTime.MinValue.Add(time.Value),
                _ => Date.Value.Date.Add(Time.Value)
            };
        set => (Date, Time) = (value?.Date, value?.TimeOfDay);
    }

    public WeightUnit Units { get; set; } = WeightUnit.g;

    public DateTime? Date { get; set; }

    public TimeSpan? Time { get; set; }

    public WeightEditModel(Weight weight) : this()
    {
        FelineId = weight.FelineId;
        Value = weight.Value;
        DateTime = weight.DateTime.UtcDateTime;
        Units = weight.Units;
    }

    public Weight ToWeight()
    {
        return new()
        {
            FelineId = FelineId,
            Value = Value,
            DateTime = DateTime!.Value,
            Units = Units
        };
    }
}

public sealed class WeightEditModelValidator : AbstractValidator<WeightEditModel>
{
    public WeightEditModelValidator()
    {
        RuleFor(model => model.FelineId)
            .GreaterThan(0)
            .WithMessage("Please select a name.");
        RuleFor(model => model.Value)
            .GreaterThan(0)
            .WithMessage("Weight must be greater than 0.");
        RuleFor(model => model.Units).IsInEnum();
        RuleFor(model => model.Date)
            .NotNull()
            .WithMessage("Please enter a date.");
        RuleFor(model => model.Time)
            .NotNull()
            .WithMessage("Please enter a time.");
    }
}

