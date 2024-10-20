namespace FosterRoster.Domain;

public sealed class FelineEditModel()
{
    public string? Breed { get; set; }
    public Category Category { get; set; }
    public string? Color { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public int Id { get; set; }
    public int? IntakeAgeInWeeks { get; set; }
    public bool IsInactive { get; init; }
    public DateTimeOffset? InactivatedAtUtc { get; init; }
    public DateTime? IntakeDate { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? RegistrationDate { get; set; }
    public Thumbnail? Thumbnail { get; set; }
    public Weaned Weaned { get; set; }

    public FelineEditModel(Feline feline) : this()
    {
        Breed = feline.Breed;
        Category = feline.Category;
        Color = feline.Color;
        Gender = feline.Gender;
        Id = feline.Id;
        IntakeAgeInWeeks = feline.IntakeAgeInWeeks;
        IntakeDate = feline.IntakeDate.ToDateTime(TimeOnly.MinValue);
        Name = feline.Name;
        RegistrationDate = feline.RegistrationDate switch
        {
            var date when date.HasValue => date.Value.ToDateTime(TimeOnly.MinValue),
            _ => null
        };
        Thumbnail = feline.Thumbnail;
        Weaned = feline.Weaned;

        IsInactive = feline.IsInactive;
        InactivatedAtUtc = feline.InactivatedAtUtc;
    }

    public Feline ToFeline()
    {
        return new()
        {
            Breed = string.IsNullOrWhiteSpace(Breed) ? null : Breed.Trim(),
            Category = Category,
            Color = string.IsNullOrWhiteSpace(Color) ? null : Color.Trim(),
            Gender = Gender,
            Id = Id,
            IntakeAgeInWeeks = IntakeAgeInWeeks,
            IntakeDate = DateOnly.FromDateTime(IntakeDate!.Value),
            Name = Name.Trim(),
            RegistrationDate = RegistrationDate switch
            {
                var date when date.HasValue => DateOnly.FromDateTime(date.Value),
                _ => null
            },
            Thumbnail = Thumbnail,
            Weaned = Weaned,
            IsInactive = IsInactive,
            InactivatedAtUtc = InactivatedAtUtc
        };
    }
}