namespace FosterRoster.Domain.Validation;

public sealed class DateTimeEditModelValidator : AbstractValidator<DateTimeEditModel>
{
    public DateTimeEditModelValidator()
    {
        RuleFor(model => model.Date)
            .NotNull()
            .WithMessage("Please enter a date.");
        RuleFor(model => model.Time)
            .NotNull()
            .WithMessage("Please enter a time.");
    }
}