namespace FosterRoster.Features.Users;

public class UserFormDto : IIdBearer
{
    /// <summary>
    ///     Password for new users or to change password for existing users.
    /// </summary>
    public string ConfirmPassword { get; set; } = string.Empty;

    /// <summary>
    ///     A list of fosterer IDs associated with the user.
    /// </summary>
    public List<int> Fosterers { get; set; } = [];

    /// <summary>
    ///     Unique ID for the user
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    ///     Unique name for the user. Must be an email address.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    ///     Email address of the user
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    ///     True if user has been locked out.
    /// </summary>
    public DateTimeOffset? LockoutEnd { [UsedImplicitly] get; set; }

    /// <summary>
    ///     Password for new users or to change password for existing users.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    ///     Optional phone number for the user
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    ///     A user can have one role.
    /// </summary>
    public string Role { get; set; } = string.Empty;
}

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

        When(x => x.IsNew, () => { RuleFor(x => x.Password).NotEmpty().WithMessage(PasswordErrorMessage); });

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
