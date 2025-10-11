namespace FosterRoster.Data;

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

[UsedImplicitly]
public sealed class FosterRosterDbContextFactory
    : IDesignTimeDbContextFactory<FosterRosterDbContext>
{
    const string DefaultConnectionString = "Host=localhost;Database=mydb;Username=myuser;Password=mypassword";

    public FosterRosterDbContext CreateDbContext(string[] args)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        var cfg = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<FosterRosterDbContext>();
        optionsBuilder.UseNpgsql(cfg.GetConnectionString("Default") ?? DefaultConnectionString);
        return new(optionsBuilder.Options);
    }
}