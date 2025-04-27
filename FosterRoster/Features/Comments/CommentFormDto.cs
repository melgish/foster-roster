namespace FosterRoster.Features.Comments;

public sealed class CommentFormDto()
{
    public int FelineId { get; init; }

    public int Id { get; init; }

    public string Text { get; set; } = string.Empty;
}