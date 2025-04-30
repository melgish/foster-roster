namespace FosterRoster.Features.Comments;

using Felines;

/// <summary>
/// Represents a single journal entry
/// </summary>
public sealed class Comment : IIdBearer
{
    /// <summary>
    ///     Feline the comment is associated with.
    /// </summary>
    public Feline Feline { get; init; } = null!;

    /// <summary>
    ///     Foreign key for the feline the comment is associated with.
    /// </summary>
    public int FelineId { get; init; }

    /// <summary>
    ///     Unique identifier for the comment.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    ///     If comment was edited, indicates the time of edit.
    /// </summary>
    public DateTimeOffset? Modified { get; set; }

    /// <summary>
    ///     HTML content of the comment.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    ///     Time comment was added to system.
    /// </summary>
    public DateTimeOffset TimeStamp { get; set; }
}