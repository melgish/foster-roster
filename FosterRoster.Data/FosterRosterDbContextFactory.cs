namespace FosterRoster.Data.Design;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class FosterRosterDbContextFactory
    : IDesignTimeDbContextFactory<FosterRosterDbContext>
{
    public FosterRosterDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FosterRosterDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=mydb;Username=myuser;Password=mypassword");
        return new FosterRosterDbContext(optionsBuilder.Options);
    }
}