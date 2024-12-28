namespace FosterRoster.Services;

public sealed class ServerSourceRepository(
    IDbContextFactory<FosterRosterDbContext> contextFactory
) : ISourceRepository
{
    /// <summary>
    ///     Adds a new source
    /// </summary>
    /// <param name="source">Source to add</param>
    /// <returns>A Result with Source if successful, or Errors on failure.</returns>
    public async Task<Result<Source>> AddAsync(Source source)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var entry = await context.Sources.AddAsync(source);
        await context.SaveChangesAsync();
        return Result.Ok(entry.Entity);
    }

    /// <summary>
    ///     Get list of all sources in the database.
    /// </summary>
    /// <returns>A Result with list of sources if successful, or Errors on failure.</returns>
    public async Task<Result<List<Source>>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return Result.Ok(await context
            .Sources
            .AsNoTracking()
            .OrderBy(s => s.Name)
            .ToListAsync()
        );
    }

    /// <summary>
    ///     Get list of all sources in the database, with only their names and ids.
    /// </summary>
    /// <returns>A Result with list of items if successful, or Errors on failure.</returns>
    public async Task<Result<List<ListItem<int>>>> GetAllNamesAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return Result.Ok(
            await context
                .Sources
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .Select(s => new ListItem<int>(s.Id, s.Name))
                .ToListAsync()
        );
    }
}