namespace FosterRoster.Extensions;

public static class DateTimeExtensions
{
    public static DateTimeOffset ToLocalTime(this DateTimeOffset dateTimeOffset, TimeProvider timeProvider)
        => TimeZoneInfo.ConvertTime(dateTimeOffset, timeProvider.LocalTimeZone);

    public static string FormatLocalTime(this DateTimeOffset dateTimeOffset, TimeProvider timeProvider,
        string format = "g")
        => ToLocalTime(dateTimeOffset, timeProvider).ToString(format);
}