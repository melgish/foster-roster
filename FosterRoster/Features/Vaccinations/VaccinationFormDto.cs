namespace FosterRoster.Features.Vaccinations;

public sealed class VaccinationFormDto : IIdBearer
{
    /// <summary>
    ///     Name of the person or organization that administered the vaccination.
    /// </summary>
    public string AdministeredBy { get; set; } = string.Empty;

    /// <summary>
    ///     Additional comments about the vaccination.
    /// </summary>
    public string? Comments { get; set; }

    /// <summary>
    ///     Date the vaccination expires.
    /// </summary>
    public DateOnly? ExpirationDate { get; set; }

    /// <summary>
    ///     Feline that received the vaccination.
    /// </summary>
    public int FelineId { get; set; }

    /// <summary>
    ///     Unique identifier for the vaccination.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    ///     Name of the vaccine manufacturer.
    /// </summary>
    public string ManufacturerName { get; set; } = string.Empty;

    /// <summary>
    ///     Serial number of the vaccine.
    /// </summary>
    public string? SerialNumber { get; set; }

    /// <summary>
    ///     Date the vaccination was administered.
    /// </summary>
    public DateOnly? VaccinationDate { get; set; }

    /// <summary>
    ///     Name of the vaccine administered.
    /// </summary>
    public string VaccineName { get; set; } = string.Empty;

    /// <summary>
    ///     When creating a new vaccination, this is the list of
    ///     felines that it will be assigned to
    /// </summary>
    public List<int> FelineIds { get; set; } = [];
}

[UsedImplicitly]
public sealed class VaccinationFormDtoValidator : AbstractValidator<VaccinationFormDto>
{
    public VaccinationFormDtoValidator(TimeProvider timeProvider)
    {
        RuleFor(model => model.AdministeredBy)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(model => model.Comments)
            .MaximumLength(256);

        RuleFor(model => model.ExpirationDate)
            .GreaterThan(model => model.VaccinationDate)
            .WithMessage("Expiration date must be after the vaccination date.");

        When(e => e.IsNew, () =>
            {
                RuleFor(e => e.FelineId)
                    .Equal(0)
                    .WithMessage("Feline must not be selected");

                RuleFor(e => e.FelineIds)
                    .Must(e => e?.Count > 0)
                    .WithMessage("At least one feline must be selected.");
            })
            .Otherwise(() =>
            {
                RuleFor(e => e.FelineId)
                    .GreaterThan(0)
                    .WithMessage("Feline must be selected");

                RuleFor(e => e.FelineIds)
                    .Must(e => e is null || e.Count == 0)
                    .WithMessage("Felines must not be selected.");
            });

        RuleFor(model => model.ManufacturerName)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(model => model.SerialNumber)
            .MaximumLength(64);

        RuleFor(model => model.VaccineName)
            .NotEmpty();

        RuleFor(model => model.VaccinationDate)
            .NotNull()
            .LessThanOrEqualTo(p => timeProvider.GetDateOnlyNow())
            .WithMessage("Vaccination date must be in the past.");
    }
}