namespace FosterRoster.Features.Schedules;

public sealed class ScheduleEditModel()
{
    public ScheduleEditModel(Schedule schedule) : this()
    {
        Cron = schedule.Cron;
        Id = schedule.Id;
        Name = schedule.Name;
    }

    public string Cron { get; set; } = string.Empty;
    
    public int Id { get; }

    public string Name { get; set; } = string.Empty;
}

[UsedImplicitly]
public sealed class ScheduleEditModelValidator : AbstractValidator<ScheduleEditModel>
{
    public ScheduleEditModelValidator(ScheduleRepository scheduleRepository)
    {
        RuleFor(m => m.Cron)
            .NotEmpty()
            .MaximumLength(128)
            .WithName("Cron Expression");
            // .MustAsync((model, cron, _) => scheduleRepository.IsUniqueCronAsync(cron, model.Id))
            // .WithMessage("{PropertyName} must be unique");

        RuleFor(m => m.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}
