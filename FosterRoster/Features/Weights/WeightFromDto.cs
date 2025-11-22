namespace FosterRoster.Features.Weights;

/// <summary>
///     A DTO for creating or updating a weight.
/// </summary>
public sealed class WeightFromDto
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

/// <summary>
///     Validator for <see cref="WeightFromDto" />.
/// </summary>
[UsedImplicitly]
public sealed class WeightFormDtoValidator : AbstractValidator<WeightFromDto>
{
    public WeightFormDtoValidator()
    {
        RuleFor(model => model.DateTime)
            .NotNull()
            .WithMessage("Please enter a date.");

        RuleFor(model => model.FelineId)
            .GreaterThan(0)
            .WithMessage("Please select a name.");

        RuleFor(model => model.Units).IsInEnum();

        RuleFor(model => model.Value)
            .GreaterThan(0)
            .WithMessage("Weight must be greater than 0.");
    }
}
