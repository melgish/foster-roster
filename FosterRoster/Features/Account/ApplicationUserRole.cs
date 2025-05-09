﻿namespace FosterRoster.Features.Account;

using Microsoft.AspNetCore.Identity;

[UsedImplicitly]
public sealed class ApplicationUserRole : IdentityUserRole<int>
{
    public ApplicationUser User { get; [UsedImplicitly] init; } = null!;
    public ApplicationRole Role { get; [UsedImplicitly] init; } = null!;
}