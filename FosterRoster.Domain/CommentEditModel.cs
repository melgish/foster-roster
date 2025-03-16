namespace FosterRoster.Domain;

public sealed class CommentEditModel()
{
    public int FelineId { get; init; }

    public int Id { get; }

    public string Text { get; set; } = string.Empty;

    public CommentEditModel(Comment comment) : this()
    {
        FelineId = comment.FelineId;
        Id = comment.Id;
        Text = comment.Text;
    }

    public Comment ToComment() =>
        new()
        {
            FelineId = FelineId,
            Id = Id,
            Text = Text
        };
}