namespace FosterRoster.Features.Chores;

[UsedImplicitly]
public sealed class ChoreCompletionFormDtoValidator : AbstractValidator<ChoreCompletionFormDto>
{
    public ChoreCompletionFormDtoValidator()
    {
        RuleFor(e => e.LogDate)
            .NotNull()
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Date must not be in the future.");
        
        RuleFor(e => e.LogText)
            .NotEmpty()
            .WithMessage("Journal entry must not be empty.");
    }
}
