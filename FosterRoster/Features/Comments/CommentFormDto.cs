namespace FosterRoster.Features.Comments;

using System.Text.RegularExpressions;

/// <summary>
///     DTO for creating or updating a journal entry.
/// </summary>
public sealed class CommentFormDto : IIdBearer
{
    public int FelineId { get; init; }

    public int Id { get; init; }

    public string Text { get; set; } = string.Empty;

    /// <summary>
    ///     If comment was edited, indicates the time of edit.
    /// </summary>
    public DateTimeOffset? Modified { get; init; }

    /// <summary>
    ///     Time comment was added to system.
    /// </summary>
    public DateTimeOffset TimeStamp { get; init; }
}

/// <summary>
///     Validation for <see cref="CommentFormDto"/>.
/// </summary>
[UsedImplicitly]
public sealed partial class CommentFormDtoValidator : AbstractValidator<CommentFormDto>
{
    public CommentFormDtoValidator()
    {
        RuleFor(model => model.FelineId)
            .GreaterThan(0);

        RuleFor(model => model.Text)
            .Must(value =>
                !string.IsNullOrWhiteSpace(value) &&
                !string.IsNullOrWhiteSpace(AnyTag.Replace(value, string.Empty)))
            .WithMessage("{PropertyName} must not be empty.")
            .MaximumLength(4000)
            .WithName("Comment");
    }

    [GeneratedRegex("<.*?>", RegexOptions.Compiled)]
    private static partial Regex AnyTag { get; }
}