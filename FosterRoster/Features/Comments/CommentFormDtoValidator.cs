namespace FosterRoster.Features.Comments;

using System.Text.RegularExpressions;

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