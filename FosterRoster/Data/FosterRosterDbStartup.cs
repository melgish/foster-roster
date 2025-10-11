namespace FosterRoster.Data;

using Features.Account;
using Features.Users;
using Microsoft.AspNetCore.Identity;

public static class FosterRosterDbStartup
{
    private class SetupConfig
    {
        public bool AutoMigrate { get; [UsedImplicitly] init; }
        public string? FirstUserEmail { get; [UsedImplicitly] init; }
        public string? FirstUserPassword { get; [UsedImplicitly] init; }
    }

    private static SetupConfig GetSetupConfig(IHost app)
    {
        var config = new SetupConfig();
        app.Services.GetRequiredService<IConfiguration>().Bind(config);
        return config;
    }

    private static async Task<bool> MigrateDatabaseAsync(IHost app, SetupConfig cfg)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<FosterRosterDbContext>();
        if (!await db.Database.CanConnectAsync())
        {
            // Database is not accepting connections.
            return false;
        }

        string[] migrations = [.. await db.Database.GetPendingMigrationsAsync()];
        if (migrations.Length == 0)
        {
            // Database is up to date.
            return true;
        }

        if (!cfg.AutoMigrate)
        {
            // Auto-migration is not enabled.
            return false;
        }

        await db.Database.MigrateAsync();
        return true;
    }

    private static async Task<bool> SeedInitialAdminUser(IHost app, SetupConfig cfg)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var manager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (await manager.Users.AnyAsync())
        {
            return true;
        }

        if (string.IsNullOrEmpty(cfg.FirstUserEmail) || string.IsNullOrEmpty(cfg.FirstUserPassword))
        {
            // No initial user data provided.
            return false;
        }

        var user = new ApplicationUser
        {
            UserName = cfg.FirstUserEmail,
            Email = cfg.FirstUserEmail,
            EmailConfirmed = true,
        };

        var rs = await manager.CreateAsync(user, cfg.FirstUserPassword);
        if (!rs.Succeeded)
        {
            // Failed to create the user.
            return false;
        }

        rs = await manager.AddToRoleAsync(user, "Admin");
        return rs.Succeeded;
    }

    /// <summary>
    /// Make sure that database is up to date and has a user
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static async Task<bool> CheckDatabaseReadyAsync(this IHost app)
    {
        var cfg = GetSetupConfig(app);
        var databaseReady = await MigrateDatabaseAsync(app, cfg);
        return databaseReady && await SeedInitialAdminUser(app, cfg);
    }
}