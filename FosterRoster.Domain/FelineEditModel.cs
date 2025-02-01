namespace FosterRoster.Domain;

public sealed class FelineEditModel()
{
    public string? Breed { get; set; }
    public Category Category { get; set; }
    public string? Color { get; set; } = string.Empty;
    public int? FostererId { get; set; }
    public Gender Gender { get; set; }
    public int Id { get; set; }
    public int? IntakeAgeInWeeks { get; set; }
    public bool IsInactive { get; init; }
    public DateTimeOffset? InactivatedAtUtc { get; init; }
    public DateOnly? IntakeDate { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly? RegistrationDate { get; set; }
    public int? SourceId { get; set; }
    public Thumbnail? Thumbnail { get; set; }
    public Weaned Weaned { get; set; }

    public FelineEditModel(Feline feline) : this()
    {
        Breed = feline.Breed;
        Category = feline.Category;
        Color = feline.Color;
        FostererId = feline.FostererId;
        Gender = feline.Gender;
        Id = feline.Id;
        IntakeAgeInWeeks = feline.IntakeAgeInWeeks;
        IntakeDate = feline.IntakeDate;
        Name = feline.Name;
        RegistrationDate = feline.RegistrationDate;
        SourceId = feline.SourceId;
        Thumbnail = feline.Thumbnail;
        Weaned = feline.Weaned;

        IsInactive = feline.IsInactive;
        InactivatedAtUtc = feline.InactivatedAtUtc;
    }

    public Feline ToFeline() =>
        new()
        {
            Breed = string.IsNullOrWhiteSpace(Breed) ? null : Breed.Trim(),
            Category = Category,
            Color = string.IsNullOrWhiteSpace(Color) ? null : Color.Trim(),
            FostererId = FostererId,
            Gender = Gender,
            Id = Id,
            IntakeAgeInWeeks = IntakeAgeInWeeks,
            IntakeDate = IntakeDate.GetValueOrDefault(),
            Name = Name.Trim(),
            RegistrationDate = RegistrationDate,
            SourceId = SourceId,
            Thumbnail = Thumbnail,
            Weaned = Weaned,
            IsInactive = IsInactive,
            InactivatedAtUtc = InactivatedAtUtc
        };
}