namespace FosterRoster.Extensions;

using System.ComponentModel.DataAnnotations;
using System.Reflection;

public static class EnumExtensions
{
    public static string ToDisplay(this Enum value)
    {
        var stringValue = value.ToString();
        return value
                .GetType()
                .GetMember(stringValue)
                .SelectMany(e => e.GetCustomAttributes(false).OfType<DisplayAttribute>())
                .FirstOrDefault() switch
            {
                { Description: {} description } => description,
                { Name: {} name } => name,
                _ => stringValue
            };
    }
}