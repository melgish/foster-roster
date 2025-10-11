namespace FosterRoster.Features.Felines;

using Comments;
using Thumbnails;

/// <summary>
///     Form data for creating or updating a <see cref="Feline" />.
/// </summary>
public sealed class FelineFormDto : IIdBearer
{
    public string? AnimalId { get; set; }
    public string? Breed { get; set; }
    public Category Category { get; set; }
    public string? Color { get; set; } = string.Empty;
    public ICollection<CommentFormDto> Comments { get; set; } = [];
    public int FostererId { get; set; }
    public Gender Gender { get; set; }
    public int Id { get; init; }
    public int? IntakeAgeInWeeks { get; set; }
    public bool IsInactive { get; init; }
    public DateTimeOffset? InactivatedAtUtc { get; init; }
    public DateOnly? IntakeDate { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly? SterilizationDate { get; set; }
    public int SourceId { get; set; }
    public Thumbnail? Thumbnail { get; set; }
    public Weaned Weaned { get; set; }
}

/// <summary>
///     Validation rules for <see cref="FelineFormDto" />.
/// </summary>
[UsedImplicitly]
public sealed class FelineFormDtoValidator : AbstractValidator<FelineFormDto>
{
    public FelineFormDtoValidator(TimeProvider timeProvider)
    {
        RuleFor(feline => feline.AnimalId)
            .MaximumLength(24);

        RuleFor(feline => feline.Breed)
            .MaximumLength(48);

        RuleFor(feline => feline.Category).IsInEnum();

        RuleFor(feline => feline.Color)
            .MaximumLength(96);

        RuleFor(feline => feline.Gender).IsInEnum();

        RuleFor(feline => feline.IntakeAgeInWeeks)
            .GreaterThanOrEqualTo(0);

        RuleFor(feline => feline.IntakeDate)
            .NotNull()
            .LessThanOrEqualTo(p => timeProvider.GetDateOnlyNow())
            .WithMessage("Intake date must be in the past.");

        RuleFor(feline => feline.Name)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(feline => feline.SterilizationDate)
            .LessThanOrEqualTo(p => timeProvider.GetDateOnlyNow())
            .WithMessage("Spay / Neuter date must be in the past.");
        
        RuleFor(feline => feline.Weaned).IsInEnum();
    }
}