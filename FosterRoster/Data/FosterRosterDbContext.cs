namespace FosterRoster.Data;

using Features.Account;
using Features.Chores;
using Features.Comments;
using Features.Felines;
using Features.Fosterers;
using Features.Microchips;
using Features.Sources;
using Features.Thumbnails;
using Features.Vaccinations;
using Features.Weights;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class FosterRosterDbContext(DbContextOptions<FosterRosterDbContext> options)
    : IdentityDbContext<
        ApplicationUser,
        ApplicationRole,
        int,
        IdentityUserClaim<int>,
        ApplicationUserRole,
        IdentityUserLogin<int>,
        IdentityRoleClaim<int>,
        IdentityUserToken<int>
    >(options), IDataProtectionKeyContext
{
    public DbSet<Chore> Chores { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Feline> Felines { get; set; } = null!;
    public DbSet<Fosterer> Fosterers { get; set; } = null!;
    public DbSet<Microchip> Microchips { get; set; } = null!;
    public DbSet<Source> Sources { get; set; } = null!;
    public DbSet<Thumbnail> Thumbnails { get; set; } = null!;
    public DbSet<Vaccination> Vaccinations { get; set; } = null!;
    public DbSet<Weight> Weights { get; set; } = null!;

    // IDataProtectionKeyContext
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(FosterRosterDbContext).Assembly);
    }
}
