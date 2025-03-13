namespace FosterRoster.Data;

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

[UsedImplicitly]
public sealed class FosterRosterDbContextFactory
    : IDesignTimeDbContextFactory<FosterRosterDbContext>
{
    public FosterRosterDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FosterRosterDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=mydb;Username=myuser;Password=mypassword");
        return new(optionsBuilder.Options);
    }
}