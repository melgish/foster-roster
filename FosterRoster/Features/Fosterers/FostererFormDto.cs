namespace FosterRoster.Features.Fosterers;

/// <summary>
///     DTO for creating or updating a fosterer.
/// </summary>
public sealed class FostererFormDto : IIdBearer
{
    /// <summary>
    ///     Mailing label style address of the Fosterer
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    ///     Gets / Sets the preferred contact method for the Fosterer
    /// </summary>
    public ContactMethod ContactMethod { get; set; } = ContactMethod.Email;

    /// <summary>
    ///     Email address of the Fosterer
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    ///     Unique identifier for the Fosterer
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    ///     Gets / Sets teh name of the Fosterer
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Primary contact phone.
    /// </summary>
    public string? Phone { get; set; }
}

/// <summary>
///     Validator for <see cref="FostererFormDto"/>.
/// </summary>
[UsedImplicitly]
public sealed class FostererFormDtoValidator : AbstractValidator<FostererFormDto>
{
    public FostererFormDtoValidator()
    {
        RuleFor(model => model.Address)
            .MaximumLength(256);

        RuleFor(model => model.ContactMethod)
            .IsInEnum();

        RuleFor(model => model.Email)
            .NotEmpty()
            .When(e => e.ContactMethod is ContactMethod.Email)
            .EmailAddress()
            .MaximumLength(64);

        RuleFor(model => model.Name)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(model => model.Phone)
            .Matches(@"\d{3}-\d{3}-\d{4}")
            .MaximumLength(16)
            .NotEmpty().When(e => e.ContactMethod is ContactMethod.Voice or ContactMethod.Text,
                ApplyConditionTo.CurrentValidator);
    }
}