namespace FosterRoster.Features.Chores;

[UsedImplicitly]
public sealed class ChoreEditModelValidator : AbstractValidator<ChoreEditModel>
{
    public ChoreEditModelValidator()
    {
        RuleFor(e => e.Description)
            .MaximumLength(256);

        RuleFor(e => e.Cron)
            .MaximumLength(128);

        RuleFor(e => e.Name)
            .MaximumLength(64)
            .NotEmpty();

        RuleFor(e => e.Repeats)
            .GreaterThan(0);
    }
}