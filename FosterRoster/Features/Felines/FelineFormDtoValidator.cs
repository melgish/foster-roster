namespace FosterRoster.Features.Felines;

[UsedImplicitly]
public sealed class FelineFormDtoValidator : AbstractValidator<FelineFormDto>
{
    public FelineFormDtoValidator()
    {
        RuleFor(feline => feline.AnimalId)
            .MaximumLength(24);

        RuleFor(feline => feline.Breed)
            .MaximumLength(48);

        RuleFor(feline => feline.Category).IsInEnum();

        RuleFor(feline => feline.Color)
            .MaximumLength(96);

        RuleFor(feline => feline.Gender).IsInEnum();

        RuleFor(feline => feline.IntakeAgeInWeeks)
            .GreaterThanOrEqualTo(0);

        RuleFor(feline => feline.IntakeDate)
            .NotNull()
            .LessThanOrEqualTo(p => GetDateOnlyNow())
            .WithMessage("Intake date must be in the past.");

        RuleFor(feline => feline.Name)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(feline => feline.SterilizationDate)
            .LessThanOrEqualTo(p => GetDateOnlyNow())
            .WithMessage("Spay / Neuter date must be in the past.");
        
        RuleFor(feline => feline.Weaned).IsInEnum();
    }

    private static DateOnly GetDateOnlyNow() => DateOnly.FromDateTime(TimeProvider.System.GetUtcNow().DateTime);
}