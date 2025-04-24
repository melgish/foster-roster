namespace FosterRoster.Features.Account;

using Data;
using Microsoft.AspNetCore.Identity;

public sealed class ApplicationUser : IdentityUser<int>, IKeyBearer
{
    public ICollection<IdentityUserRole<int>> UserRoles { get; set; } = [];
}