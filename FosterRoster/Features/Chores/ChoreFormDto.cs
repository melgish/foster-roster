namespace FosterRoster.Features.Chores;

/// <summary>
///     DTO for creating or updating a chore.
/// </summary>
public sealed class ChoreFormDto : IIdBearer
{
    /// <summary>
    ///     Description of the task. Description will be added
    ///     to journal entry when task is completed.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     Date task is due. If null, no due date will be assigned.
    /// </summary>
    public DateTimeOffset? DueDate { get; set; }

    /// <summary>
    ///     Feline associated with the task. If null, the task is
    ///     considered a template task that can be cloned for
    ///     any feline.
    /// </summary>
    public int FelineId { get; set; }

    /// <summary>
    ///     Name of task to display to the user.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     When creating a new task, this is the list of
    ///     felines that it will be assigned to
    /// </summary>
    public List<int> FelineIds { get; set; } = [];

    /// <summary>
    ///     Unique identifier for the task.
    /// </summary>
    public int Id { get; init; }
}

/// <summary>
///     Validator for <see cref="ChoreFormDto" />.
/// </summary>
[UsedImplicitly]
public sealed class ChoreFormDtoValidator : AbstractValidator<ChoreFormDto>
{
    public ChoreFormDtoValidator()
    {
        RuleFor(e => e.Description)
            .MaximumLength(256);

        RuleFor(e => e.Name)
            .MaximumLength(64)
            .NotEmpty();

        When(e => e.IsNew, () =>
            {
                RuleFor(e => e.FelineId)
                    .Must(e => e == 0)
                    .WithMessage("Feline must not be selected");

                RuleFor(e => e.FelineIds)
                    .Must(e => e?.Count > 0)
                    .WithMessage("At least one feline must be selected.");
            })
            .Otherwise(() =>
            {
                RuleFor(e => e.FelineId)
                    .Must(e => e != 0)
                    .WithMessage("Feline must be selected");

                RuleFor(e => e.FelineIds)
                    .Must(e => e is null || e.Count == 0)
                    .WithMessage("Felines must not be selected.");
            });
    }
}
