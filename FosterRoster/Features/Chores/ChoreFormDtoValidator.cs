namespace FosterRoster.Features.Chores;

[UsedImplicitly]
public sealed class ChoreFormDtoValidator : AbstractValidator<ChoreFormDto>
{
    public ChoreFormDtoValidator()
    {
        RuleFor(e => e.Description)
            .MaximumLength(256);

        RuleFor(e => e.Name)
            .MaximumLength(64)
            .NotEmpty();

        When(e => e.Id == 0, () =>
        {
            RuleFor(e => e.FelineId)
                .Must(e => e == 0)
                .WithMessage("Feline must not be selected");
            
            RuleFor(e => e.FelineIds)
                .Must(e => e.Count > 0)
                .WithMessage("At least one feline must be selected.");
        })
        .Otherwise(() =>
        {
            RuleFor(e => e.FelineId)
                .Must(e => e != 0)
                .WithMessage("Feline must be selected");

            RuleFor(e => e.FelineIds)
                .Must(e => e.Count == 0)
                .WithMessage("Felines must not be selected.");
        });
    }
}