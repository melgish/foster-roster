namespace FosterRoster.Features.Account;

using Microsoft.AspNetCore.Identity;

[UsedImplicitly]
public sealed class ApplicationUserRole : IdentityUserRole<int>
{
    public ApplicationUser User { get; set; } = null!;
    public ApplicationRole Role { get; set; } = null!;
}