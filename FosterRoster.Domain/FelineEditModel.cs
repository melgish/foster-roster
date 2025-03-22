namespace FosterRoster.Domain;

public sealed class FelineEditModel()
{
    public FelineEditModel(Feline feline) : this()
    {
        AnimalId = feline.AnimalId;
        Breed = feline.Breed;
        Category = feline.Category;
        Color = feline.Color;
        FostererId = feline.FostererId.GetValueOrDefault();
        Gender = feline.Gender;
        Id = feline.Id;
        IntakeAgeInWeeks = feline.IntakeAgeInWeeks;
        IntakeDate = feline.IntakeDate;
        Name = feline.Name;
        RegistrationDate = feline.RegistrationDate;
        SourceId = feline.SourceId.GetValueOrDefault();
        Thumbnail = feline.Thumbnail;
        Weaned = feline.Weaned;

        IsInactive = feline.IsInactive;
        InactivatedAtUtc = feline.InactivatedAtUtc;
    }

    public string? AnimalId { get; set; }
    public string? Breed { get; set; }
    public Category Category { get; set; }
    public string? Color { get; set; } = string.Empty;
    public int FostererId { get; set; }
    public Gender Gender { get; set; }
    public int Id { get; }
    public int? IntakeAgeInWeeks { get; set; }
    public bool IsInactive { get; }
    public DateTimeOffset? InactivatedAtUtc { get; }
    public DateOnly? IntakeDate { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly? RegistrationDate { get; set; }
    public int SourceId { get; set; }
    public Thumbnail? Thumbnail { get; set; }
    public Weaned Weaned { get; set; }

    public Feline ToFeline() =>
        new()
        {
            AnimalId = string.IsNullOrWhiteSpace(AnimalId) ? null : AnimalId.Trim(),
            Breed = string.IsNullOrWhiteSpace(Breed) ? null : Breed.Trim(),
            Category = Category,
            Color = string.IsNullOrWhiteSpace(Color) ? null : Color.Trim(),
            FostererId = FostererId == 0 ? null : FostererId,
            Gender = Gender,
            Id = Id,
            IntakeAgeInWeeks = IntakeAgeInWeeks,
            IntakeDate = IntakeDate.GetValueOrDefault(),
            Name = Name.Trim(),
            RegistrationDate = RegistrationDate,
            SourceId = SourceId == 0 ? null : SourceId,
            Thumbnail = Thumbnail,
            Weaned = Weaned,
            IsInactive = IsInactive,
            InactivatedAtUtc = InactivatedAtUtc
        };
}