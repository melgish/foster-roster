namespace FosterRoster.Infrastructure;

public static class DataExtensions
{
    /// <summary>
    /// Collapse strings to null if they are empty or whitespace.
    /// </summary>
    /// <param name="value">Transform value to null if empty after trim</param>
    /// <returns>Trimmed string or null</returns>
    public static string TrimToNull(this string? value)
        => string.IsNullOrWhiteSpace(value) ? null! : value.Trim();

    /// <summary>
    /// Convert value to null when it is zero.
    /// </summary>
    /// <param name="value">Value to convert</param>
    /// <returns></returns>
    public static int? ZeroToNull(this int value)
        => value == 0 ? null : value;
}