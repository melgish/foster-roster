namespace FosterRoster.Infrastructure;

public class TexasTimeProvider : TimeProvider
{
    public override TimeZoneInfo LocalTimeZone { get; } = TimeZoneInfo
        .FindSystemTimeZoneById("Central Standard Time");
}

public static class TimeProviderExtensions
{
    public static DateOnly GetDateOnlyNow(this TimeProvider timeProvider) =>
        DateOnly.FromDateTime(timeProvider.GetUtcNow().DateTime);
}