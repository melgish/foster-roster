namespace FosterRoster.Features.Account;

using Microsoft.AspNetCore.Identity;

public sealed class ApplicationRole : IdentityRole<int>, IIdBearer
{
    public ICollection<ApplicationUserRole> UserRoles { get; init; } = [];
}