namespace FosterRoster.Features.Users;

[UsedImplicitly]
public sealed class UserFromDtoValidator : AbstractValidator<UserFormDto>
{
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
        
        When(x => x.Id == 0, () =>
            {
                RuleFor(x => x.Password)
                    .NotEmpty()
                    .MinimumLength(6)
                    .WithMessage("Password must be at least 6 characters long.");

                RuleFor(x => x.ConfirmPassword)
                    .Equal(x => x.Password)
                    .WithMessage("Passwords do not match.");
            })
            .Otherwise(() =>
            {
                RuleFor(x => x.Password)
                    .MinimumLength(6)
                    .WithMessage("Password must be at least 6 characters long.");

                RuleFor(x => x.ConfirmPassword)
                    .Equal(x => x.Password)
                    .WithMessage("Passwords do not match.");
            });
    }
}