namespace FosterRoster.Domain;

public sealed class SourceEditModel()
{
    public SourceEditModel(Source source) : this()
    {
        Id = source.Id;
        Name = source.Name;
    }

    private int Id { get; }

    public string Name { get; set; } = string.Empty;

    public Source ToSource() =>
        new()
        {
            Id = Id,
            Name = Name
        };
}