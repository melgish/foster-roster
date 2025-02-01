namespace FosterRoster.Client.Extensions;

public static class FelineExtensions
{
    private static string FormatAge(int? ageInWeeks, DateOnly? intakeDate, DateTimeOffset asOfDate)
        => FormatAge(ageInWeeks, intakeDate?.ToDateTime(TimeOnly.MinValue), asOfDate);

    private static string FormatAge(int? ageInWeeks, DateTime? intakeDate, DateTimeOffset asOfDate)
    {
        TimeSpan? age = null;
        if (ageInWeeks.HasValue && intakeDate.HasValue)
            age = TimeSpan.FromDays(ageInWeeks.Value * 7) + (asOfDate - intakeDate.Value);
        return age switch
        {
            { Days: <= 7 } d => $"{d.Days} days old",
            { Days: <= 180 } d => $"{d.Days / 7:F0} weeks old",
            { Days: <= 730 } d => $"{d.Days / 30:F0} months old",
            { Days: > 730 } d => $"{d.Days / 365.25:F0} years old",
            _ => "Age unknown"
        };
    }

    public static string FormatAge(this FelineEditModel? feline, DateTimeOffset asOfDate)
        => FormatAge(feline?.IntakeAgeInWeeks, feline?.IntakeDate, asOfDate);

    public static string FormatAge(this Feline? feline, DateTimeOffset asOfDate)
        => FormatAge(feline?.IntakeAgeInWeeks, feline?.IntakeDate.ToDateTime(TimeOnly.MinValue), asOfDate);
}