namespace FosterRoster.Domain;

public class Comment
{
    public virtual Feline Feline { get; init; } = null!;
    public int FelineId { get; init; }
    public int Id { get; init; }
    public DateTimeOffset? Modified { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTimeOffset TimeStamp { get; set; }
}