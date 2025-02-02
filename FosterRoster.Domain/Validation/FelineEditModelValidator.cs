namespace FosterRoster.Domain.Validation;

[UsedImplicitly]
public sealed class FelineEditModelValidator : AbstractValidator<FelineEditModel>
{
    public FelineEditModelValidator(TimeProvider timeProvider)
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
            .LessThanOrEqualTo(p => DateOnly.FromDateTime(timeProvider.GetUtcNow().DateTime))
            .WithMessage("Intake date must be in the past.");

        RuleFor(feline => feline.Name)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(feline => feline.RegistrationDate)
            .LessThanOrEqualTo(p => DateOnly.FromDateTime(timeProvider.GetUtcNow().DateTime))
            .WithMessage("Registration date must be in the past.");

        RuleFor(feline => feline.Weaned).IsInEnum();
    }
}