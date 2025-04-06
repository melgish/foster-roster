namespace FosterRoster.Data;

using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Features.Account;
using Features.Chores;
using Features.Felines;
using Features.Fosterers;
using Features.Sources;
using Features.Weights;

public class FosterRosterDbContext(DbContextOptions<FosterRosterDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, int>(options), IDataProtectionKeyContext
{
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Feline> Felines { get; set; } = null!;
    public DbSet<Fosterer> Fosterers { get; set; } = null!;
    public DbSet<Source> Sources { get; set; } = null!;
    public DbSet<Chore> Chores { get; set; } = null!;
    public DbSet<Thumbnail> Thumbnails { get; set; } = null!;
    public DbSet<Weight> Weights { get; set; } = null!;

    // IDataProtectionKeyContext
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FosterRosterDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}