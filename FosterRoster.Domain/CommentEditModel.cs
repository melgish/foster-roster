namespace FosterRoster.Domain;

public sealed class CommentEditModel()
{
    public int FelineId { get; set; }

    public int Id { get; init; }

    public string Text { get; set; } = string.Empty;

    public DateTimeOffset TimeStamp { get; init; }


    public CommentEditModel(Comment comment) : this()
    {
        FelineId = comment.FelineId;
        Id = comment.Id;
        Text = comment.Text;
        TimeStamp = comment.TimeStamp;
    }

    public Comment ToComment() =>
        new()
        {
            FelineId = FelineId,
            Id = Id,
            Text = Text,
            TimeStamp = TimeStamp
        };
}