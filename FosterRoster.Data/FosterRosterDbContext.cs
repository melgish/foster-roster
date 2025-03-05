using FosterRoster.Domain;
using Microsoft.EntityFrameworkCore;

namespace FosterRoster.Data;

using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class FosterRosterDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>, IDataProtectionKeyContext

{
    public FosterRosterDbContext(DbContextOptions<FosterRosterDbContext> options) : base(options)
    {
    }

    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Feline> Felines { get; set; } = null!;
    public DbSet<Fosterer> Fosterers { get; set; } = null!;
    public DbSet<Source> Sources { get; set; } = null!;
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