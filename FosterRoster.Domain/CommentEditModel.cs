namespace FosterRoster.Domain;

public sealed class CommentEditModel()
{
    public int FelineId { get; init; }

    public int Id { get; init; }

    public DateTimeOffset? Modified { get; set; }

    public string Text { get; set; } = string.Empty;

    public DateTimeOffset TimeStamp { get; init; }

    public CommentEditModel(Comment comment) : this()
    {
        FelineId = comment.FelineId;
        Id = comment.Id;
        Modified = comment.Modified;
        Text = comment.Text;
        TimeStamp = comment.TimeStamp;
    }

    public Comment ToComment() =>
        new()
        {
            FelineId = FelineId,
            Id = Id,
            Modified = Modified,
            Text = Text,
            TimeStamp = TimeStamp
        };
}