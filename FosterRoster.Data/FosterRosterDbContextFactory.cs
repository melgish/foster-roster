using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FosterRoster.Data;

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