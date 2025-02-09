namespace FosterRoster.Domain.Validation;

[UsedImplicitly]
public sealed class SourceEditModelValidator : AbstractValidator<SourceEditModel>
{
    public SourceEditModelValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty()
            .MaximumLength(64);
    }
}