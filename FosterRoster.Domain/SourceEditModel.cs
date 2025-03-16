namespace FosterRoster.Domain;

public sealed class SourceEditModel()
{
    private int Id { get; }

    public string Name { get; set; } = string.Empty;

    public SourceEditModel(Source source) : this()
    {
        Id = source.Id;
        Name = source.Name;
    }

    public Source ToSource() =>
        new()
        {
            Id = Id,
            Name = Name
        };
}