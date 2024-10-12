namespace FosterRoster.Domain;

using FluentValidation;

public sealed class WeightEditModel()
{
    public int FelineId { get; set; }
    public float Value { get; set; }

    // This is a computed property that combines the Date and Time properties
    // because MudBlazor does not have a Date+Time picker.
    public DateTimeEditModel DateTime { get; set; } = new();

    public WeightUnit Units { get; set; } = WeightUnit.g;

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
            DateTime = DateTime.Value!.Value,
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
        RuleFor(model => model.DateTime.Date)
            .NotNull()
            .WithMessage("Please enter a date.");
        RuleFor(model => model.DateTime.Time)
            .NotNull()
            .WithMessage("Please enter a time.");
    }
}

