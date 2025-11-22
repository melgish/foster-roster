namespace FosterRoster.Infrastructure;

public static class DataExtensions
{
    /// <param name="value">Transform value to null if empty after trim</param>
    extension(string? value)
    {
        /// <summary>
        /// Collapse strings to null if they are empty or whitespace.
        /// </summary>
        /// <returns>Trimmed string or null</returns>
        public string TrimToNull() => string.IsNullOrWhiteSpace(value) ? null! : value.Trim();
    }

    /// <param name="value">Value to convert</param>
    extension(int value)
    {
        /// <summary>
        /// Convert value to null when it is zero.
        /// </summary>
        /// <returns></returns>
        public int? ZeroToNull() => value == 0 ? null : value;
    }
}
