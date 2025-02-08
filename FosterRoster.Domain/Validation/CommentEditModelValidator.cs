namespace FosterRoster.Domain.Validation;

using System.Text.RegularExpressions;

[UsedImplicitly]
public sealed class CommentEditModelValidator : AbstractValidator<CommentEditModel>
{
    private static readonly Regex AnyTag = new("<.*?>", RegexOptions.Compiled);

    public CommentEditModelValidator()
    {
        RuleFor(model => model.FelineId)
            .GreaterThan(0);

        RuleFor(model => model.Text)
            .Must((value) =>
                !string.IsNullOrWhiteSpace(value) &&
                !string.IsNullOrWhiteSpace(AnyTag.Replace(value, string.Empty)))
            .WithMessage("{PropertyName} must not be empty.")
            .MaximumLength(4000)
            .WithName("Comment");
    }
}