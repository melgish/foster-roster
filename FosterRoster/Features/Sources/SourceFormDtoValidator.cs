namespace FosterRoster.Features.Sources;

[UsedImplicitly]
public sealed class SourceFormDtoValidator : AbstractValidator<SourceFormDto>
{
    public SourceFormDtoValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty()
            .MaximumLength(64);
    }
}