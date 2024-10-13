namespace FosterRoster.Domain;

using FluentValidation;

/// <summary>
/// A model for dealing with MudBlazor Date / Time picker quirks.
/// </summary>
public sealed class DateTimeEditModel
{
    /// <summary>
    /// Gets or sets the combined date and time value.
    /// </summary>
    public DateTime? Value
    {
        get => (Date, Time) switch
        {
            (null, null) => default(DateTime?),
            (var date, null) => date.Value.Date,
            (null, var time) => DateTime.MinValue.Add(time.Value),
            _ => Date.Value.Date.Add(Time.Value)
        };
        set => (Date, Time) = (value?.Date, value?.TimeOfDay);
    }

    public DateTimeEditModel(DateTime? value = null)
    {
        Value = value;
    }

    /// <summary>
    /// Gets or sets the date part of the value.
    /// </summary>
    public DateTime? Date { get; set; }

    /// <summary>
    /// Gets or sets the time part of the value.
    /// </summary>
    public TimeSpan? Time { get; set; }


    public static implicit operator DateTime?(DateTimeEditModel model) => model.Value;
    public static implicit operator DateTime(DateTimeEditModel model) => model.Value!.Value;
    public static implicit operator DateTimeEditModel(DateTime? value) => new(value);
}

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


