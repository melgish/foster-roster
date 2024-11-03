namespace FosterRoster.Services;

public sealed class ServerSourceRepository(
    IDbContextFactory<FosterRosterDbContext> contextFactory
) : ISourceRepository
{
    public async Task<Source> AddAsync(Source source)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var entry = await context.Sources.AddAsync(source);
        await context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<List<Source>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context
            .Sources
            .AsNoTracking()
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<List<ListItem<int>>> GetAllNamesAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context
            .Sources
            .AsNoTracking()
            .OrderBy(s => s.Name)
            .Select(s => new ListItem<int>(s.Id, s.Name))
            .ToListAsync();
    }
}
