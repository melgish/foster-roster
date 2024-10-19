namespace FosterRoster.Domain;

public class Feline : IInactivatable
{
    public string? Breed { get; set; }
    public Category Category { get; set; }
    public string? Color { get; set; }
    public virtual ICollection<Comment> Comments { get; set; } = [];
    public Gender Gender { get; set; }
    public int Id { get; set; }
    public DateTimeOffset? InactivatedAtUtc { get; set; }
    public int? IntakeAgeInWeeks { get; set; }
    public DateOnly IntakeDate { get; set; }
    public bool IsInactive { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly? RegistrationDate { get; set; }
    public virtual Thumbnail? Thumbnail { get; set; }
    public Weaned Weaned { get; set; }
    public virtual ICollection<Weight> Weights { get; set; } = [];
}

