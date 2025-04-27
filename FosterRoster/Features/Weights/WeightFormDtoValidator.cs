namespace FosterRoster.Features.Weights;

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