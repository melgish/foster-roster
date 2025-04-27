namespace FosterRoster.Features.Schedules;

[UsedImplicitly]
public sealed class ScheduleFormDtoValidator : AbstractValidator<ScheduleFormDto>
{
    public ScheduleFormDtoValidator(ScheduleRepository scheduleRepository)
    {
        RuleFor(m => m.Cron)
            .NotEmpty()
            .MaximumLength(128)
            .WithName("Cron Expression");

        RuleFor(m => m.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}