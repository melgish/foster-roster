namespace FosterRoster.Features.Comments;

public sealed class CommentFormDto : IIdBearer
{
    public int FelineId { get; init; }

    public int Id { get; init; }

    public string Text { get; set; } = string.Empty;

    /// <summary>
    ///     If comment was edited, indicates the time of edit.
    /// </summary>
    public DateTimeOffset? Modified { get; init; }

    /// <summary>
    ///     Time comment was added to system.
    /// </summary>
    public DateTimeOffset TimeStamp { get; init; }
}