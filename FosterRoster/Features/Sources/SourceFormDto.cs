namespace FosterRoster.Features.Sources;

/// <summary>
///     A DTO for creating or updating a source.
/// </summary>
public sealed class SourceFormDto : IIdBearer
{
    /// <summary>
    ///     Unique identifier for the source.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    ///     Name for the source.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}

/// <summary>
///     Validator for <see cref="SourceFormDto"/>.
/// </summary>
[UsedImplicitly]
public sealed class SourceFormDtoValidator : AbstractValidator<SourceFormDto>
{
    public SourceFormDtoValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty()
            .MaximumLength(64);
    }
}