namespace FosterRoster.Infrastructure;

using System.ComponentModel.DataAnnotations;

public static class EnumExtensions
{
    extension(Enum value)
    {
        /// <summary>
        ///     Use display attribute to get a human-readable name for an enum value.
        /// </summary>
        /// <returns></returns>
        public string ToDisplay()
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
}