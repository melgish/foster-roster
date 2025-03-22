namespace FosterRoster.Extensions;

public static class DateTimeExtensions
{
    /// <summary>
    ///     Uses supplied timeProvider to convert time to local time.
    /// </summary>
    /// <param name="dateTimeOffset">Time to convert</param>
    /// <param name="timeProvider">TimeProvider with timezone to use for conversion.</param>
    /// <returns>New instance converted to offset.</returns>
    private static DateTimeOffset ToLocalTime(this DateTimeOffset dateTimeOffset, TimeProvider timeProvider)
        => TimeZoneInfo.ConvertTime(dateTimeOffset, timeProvider.LocalTimeZone);

    /// <summary>
    ///     Uses supplied timeProvider to format time as local time.
    /// </summary>
    /// <param name="dateTimeOffset">Time to display</param>
    /// <param name="timeProvider">TimeProvider with timezone to use for conversion.</param>
    /// <param name="format">Output format</param>
    /// <returns>Formatted time value</returns>
    public static string FormatLocalTime(this DateTimeOffset dateTimeOffset, TimeProvider timeProvider,
        string format = "g")
        => ToLocalTime(dateTimeOffset, timeProvider).ToString(format);

    /// <summary>
    ///     Formats a DateTimeOffset as a relative time string.
    /// </summary>
    /// <param name="dateTimeOffset">Time to display</param>
    /// <param name="asOfDate">Date to base ago off of</param>
    /// <returns>Formatted time value</returns>
    public static string FormatAgo(this DateTimeOffset dateTimeOffset, DateTimeOffset asOfDate)
        => (asOfDate - dateTimeOffset) switch
        {
            { Days: > 1 } d => $"{d.Days:F0} days ago",
            { Hours: > 1 } d => $"{d.Hours:F0} hours ago",
            { Minutes: > 1 } d => $"{d.Minutes:F0} minutes ago",
            { Seconds: > 1 } d => $"{d.Seconds:F0} seconds ago",
            _ => "now"
        };
}