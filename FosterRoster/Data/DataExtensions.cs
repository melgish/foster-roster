namespace FosterRoster.Data;

public static class DataExtensions
{
    /// <summary>
    /// Collapse strings to null if they are empty or whitespace.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string TrimToNull(this string? value)
        => string.IsNullOrWhiteSpace(value) ? null! : value.Trim();
    
    public static int? ZeroToNull(this int? value)
        => value == 0 ? null : value;

    public static int? ZeroToNull(this int value)
        => value == 0 ? null : value; 

}