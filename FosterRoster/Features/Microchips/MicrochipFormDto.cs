namespace FosterRoster.Features.Microchips;

public sealed class MicrochipFormDto
{
    /// <summary>
    ///     Brand of the microchip.
    /// </summary>
    public string Brand { get; set; } = string.Empty;

    /// <summary>
    ///     Extra comments about the microchip.
    /// </summary>
    public string? Comment { get; set; }
    
    /// <summary>
    ///     Foreign key for the feline the chip is associated with.
    /// </summary>
    public int FelineId { get; set; }
    
    /// <summary>
    ///     Unique identifier for the relation.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    ///     The chip code / number.
    /// </summary>
    public string MicrochipId { get; set; } = string.Empty;
}

[UsedImplicitly]
internal sealed class MicrochipFormDtoValidator : AbstractValidator<MicrochipFormDto>
{
    public MicrochipFormDtoValidator()
    {
        RuleFor(m => m.Brand)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(m => m.Comment)
            .MaximumLength(256);

        RuleFor(m => m.FelineId)
            .GreaterThan(0)
            .WithMessage("Chip must be assigned to a Feline");

        RuleFor(m => m.MicrochipId)
            .NotEmpty()
            .MaximumLength(24);
    }
}