namespace FosterRoster.Features.Fosterers;

[UsedImplicitly]
public sealed class FostererFormDtoValidator : AbstractValidator<FostererFormDto>
{
    public FostererFormDtoValidator()
    {
        RuleFor(model => model.Address)
            .MaximumLength(256);

        RuleFor(model => model.ContactMethod)
            .IsInEnum();

        RuleFor(model => model.Email)
            .NotEmpty().When(e => e.ContactMethod is ContactMethod.Email)
            .EmailAddress()
            .MaximumLength(64);

        RuleFor(model => model.Name)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(model => model.Phone)
            .Matches(@"\d{3}-\d{3}-\d{4}")
            .MaximumLength(16)
            .NotEmpty().When(e => e.ContactMethod is ContactMethod.Voice or ContactMethod.Text,
                ApplyConditionTo.CurrentValidator);
    }
}