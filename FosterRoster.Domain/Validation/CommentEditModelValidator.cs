namespace FosterRoster.Domain.Validation;

public sealed class CommentEditModelValidator : AbstractValidator<CommentEditModel>
{
    public CommentEditModelValidator()
    {
        RuleFor(model => model.FelineId)
            .GreaterThan(0);

        RuleFor(model => model.Text)
            .NotEmpty()
            .MaximumLength(4000);
    }
}