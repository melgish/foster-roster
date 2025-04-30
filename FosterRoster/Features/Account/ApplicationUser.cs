namespace FosterRoster.Features.Account;

using Microsoft.AspNetCore.Identity;

public sealed class ApplicationUser : IdentityUser<int>, IIdBearer
{
    public ICollection<ApplicationUserRole> UserRoles { get; set; } = [];
}