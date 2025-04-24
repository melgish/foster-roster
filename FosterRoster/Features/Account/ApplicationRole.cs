namespace FosterRoster.Features.Account;

using Data;
using Microsoft.AspNetCore.Identity;

public sealed class ApplicationRole : IdentityRole<int>, IKeyBearer;