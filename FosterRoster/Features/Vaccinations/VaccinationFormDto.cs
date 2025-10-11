namespace FosterRoster.Features.Vaccinations;

using Felines;

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
    public Feline Feline { get; init; } = null!;

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
    public string SerialNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Date the vaccination was administered.
    /// </summary>
    public DateOnly? VaccinationDate { get; set; }

    /// <summary>
    ///     Name of the vaccine administered.
    /// </summary>
    public string VaccineName { get; set; } = string.Empty;
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
            .NotNull()
            .GreaterThan(model => model.VaccinationDate)
            .WithMessage("Expiration date must be after the vaccination date.");

        RuleFor(model => model.FelineId)
            .GreaterThan(0)
            .WithMessage("Please select a feline.");

        RuleFor(model => model.ManufacturerName)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(model => model.SerialNumber)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(model => model.VaccineName)
            .NotEmpty();

        RuleFor(model => model.VaccinationDate)
            .NotNull()
            .LessThanOrEqualTo(p => timeProvider.GetDateOnlyNow())
            .WithMessage("Vaccination date must be in the past.");
    }
}