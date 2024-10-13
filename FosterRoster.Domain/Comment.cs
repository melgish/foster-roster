namespace FosterRoster.Domain;

public class Comment
{
    public virtual Feline Feline { get; set; } = null!;
    public int FelineId { get; set; }
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTimeOffset TimeStamp { get; set; }
}

