namespace FosterRoster.Domain.Validation;

[UsedImplicitly]
public sealed class SourceValidator : AbstractValidator<Source>
{
    public SourceValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty()
            .MaximumLength(64);
    }
}