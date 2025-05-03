namespace FosterRoster.Features.Users;

[UsedImplicitly]
public sealed class UserFromDtoValidator : AbstractValidator<UserFormDto>
{
    private const string PasswordErrorMessage =
        "Password must be at least 6 characters long and contain at least one non-alphanumeric character";

    public UserFromDtoValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("User name must be a valid email address.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email must be a valid email address.");

        When(x => x.Id == 0, () => { RuleFor(x => x.Password).NotEmpty().WithMessage(PasswordErrorMessage); });

        When(x => !string.IsNullOrEmpty(x.Password), () =>
        {
            RuleFor(x => x.Password)
                .MinimumLength(6).WithMessage(PasswordErrorMessage)
                .Matches("[a-z]").WithMessage(PasswordErrorMessage)
                .Matches("[^a-zA-Z0-9]").WithMessage(PasswordErrorMessage);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Passwords do not match.");
        });

        When(x => string.IsNullOrEmpty(x.Password), () =>
        {
            RuleFor(x => x.ConfirmPassword)
                .Empty()
                .WithMessage("Confirm password must be empty if password is empty.");
        });
    }
}