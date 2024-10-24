namespace FosterRoster.Services;

public sealed class ServerFostererRepository(
    IDbContextFactory<FosterRosterDbContext> contextFactory
) : IFostererRepository
{
    public async Task<List<Fosterer>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context
            .Fosterers
            .AsNoTracking()
            .OrderBy(f => f.Name)
            .ToListAsync();
    }

    public async Task<List<ListItem<int>>> GetAllNamesAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context
            .Fosterers
            .AsNoTracking()
            .OrderBy(f => f.Name)
            .Select(f => new ListItem<int>(f.Id, f.Name))
            .ToListAsync();
    }
}