namespace FosterRoster.Services;

public class TexasTimeProvider : TimeProvider
{
    public override TimeZoneInfo LocalTimeZone { get; } = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
}