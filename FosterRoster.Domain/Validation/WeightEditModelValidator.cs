namespace FosterRoster.Domain.Validation;

[UsedImplicitly]
public sealed class WeightEditModelValidator : AbstractValidator<WeightEditModel>
{
    public WeightEditModelValidator()
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