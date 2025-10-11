namespace FosterRoster.Infrastructure;

using System.ComponentModel.DataAnnotations;

public static class EnumExtensions
{
    /// <summary>
    ///     Use display attribute to get a human-readable name for an enum value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToDisplay(this Enum value)
    {
        var stringValue = value.ToString();
        return value
                .GetType()
                .GetMember(stringValue)
                .SelectMany(e => e.GetCustomAttributes(false).OfType<DisplayAttribute>())
                .FirstOrDefault() switch
        {
            { Description: { } description } => description,
            { Name: { } name } => name,
            _ => stringValue
        };
    }
}