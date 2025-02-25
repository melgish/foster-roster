namespace FosterRoster.Extensions;

public static class CommentExtensions
{
    public static string FormatAgo(this Comment comment, DateTimeOffset asOfDate)
        => comment.TimeStamp.FormatAgo(asOfDate);

    public static string FormatAgo(this DateTimeOffset date, DateTimeOffset asOfDate)
        => (asOfDate - date) switch
        {
            { Days: > 1 } d => $"{d.Days:F0} days ago",
            { Hours: > 1 } d => $"{d.Hours:F0} hours ago",
            { Minutes: > 1 } d => $"{d.Minutes:F0} minutes ago",
            { Seconds: > 1 } d => $"{d.Seconds:F0} seconds ago",
            _ => "now"
        };
}