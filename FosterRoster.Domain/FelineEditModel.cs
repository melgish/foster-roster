namespace FosterRoster.Domain;

using FluentValidation;

public sealed class FelineEditModel()
{
    public string? Breed { get; set; }
    public Category Category { get; set; }
    public Gender Gender { get; set; }
    public int Id { get; set; }
    public int? IntakeAgeInWeeks { get; set; }
    public DateTime? IntakeDate { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? RegistrationDate { get; set; }
    public Thumbnail? Thumbnail { get; set; }
    public Weaned Weaned { get; set; }

    public FelineEditModel(Feline feline): this()
    {
        Breed = feline.Breed;
        Category = feline.Category;
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
    }

    public Feline ToFeline()
    {
        return new()
        {
            Breed = Breed?.Trim(),
            Category = Category,
            Gender = Gender,
            Id = Id,
            IntakeAgeInWeeks = IntakeAgeInWeeks,
            IntakeDate = DateOnly.FromDateTime(IntakeDate!.Value),
            Name = Name.Trim(),
            RegistrationDate = RegistrationDate switch {
                var date when date.HasValue => DateOnly.FromDateTime(date.Value),
                _ => null
            },
            Thumbnail = Thumbnail,
            Weaned = Weaned,
        };
    }
}

public sealed class FelineEditModelValidator : AbstractValidator<FelineEditModel>
{
    public FelineEditModelValidator(TimeProvider timeProvider)
    {
        RuleFor(feline => feline.Breed)
            .MaximumLength(48);

        RuleFor(feline => feline.Category).IsInEnum();

        RuleFor(feline => feline.Gender).IsInEnum();

        RuleFor(feline => feline.IntakeAgeInWeeks)
            .GreaterThanOrEqualTo(0);

        RuleFor(feline => feline.IntakeDate)
            .NotNull()
            .LessThanOrEqualTo(p => timeProvider.GetUtcNow().DateTime)
            .WithMessage("Intake date must be in the past.");

        RuleFor(feline => feline.Name)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(feline => feline.Weaned).IsInEnum();

        RuleFor(feline => feline.RegistrationDate)
            .LessThanOrEqualTo(p => timeProvider.GetUtcNow().DateTime)
            .WithMessage("Registration date must be in the past.");}
}
