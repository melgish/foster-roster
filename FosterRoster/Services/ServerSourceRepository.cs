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
    ///     Deletes a Source by its ID.
    /// </summary>
    /// <param name="sourceId">ID of source to remove.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> DeleteByKeyAsync(int sourceId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Sources.Where(s => s.Id == sourceId).ExecuteDeleteAsync() switch
        {
            0 => Result.Fail(new NotFoundError()),
            1 => Result.Ok(),
            _ => Result.Fail(new MultipleChangesError())
        };
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
    ///     Gets single Source from the database.
    /// </summary>
    /// <param name="sourceId">ID of source to return.</param>
    /// <returns>Result with Source if successful, or Errors on failure.</returns>
    public async Task<Result<Source>> GetByKeyAsync(int sourceId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context
                .Sources
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == sourceId) switch
            {
                null => Result.Fail(new NotFoundError()),
                { } source => Result.Ok(source)
            };
    }

    /// <summary>
    ///     Updates an existing Source in the database.
    /// </summary>
    /// <param name="sourceId">ID of source to update.</param>
    /// <param name="source">Data to assign to Source</param>
    /// <returns>Result with updated Source if found, or Errors on failure.</returns>
    public async Task<Result<Source>> UpdateAsync(int sourceId, Source source)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var existing = await context.Sources.FindAsync(sourceId);
        if (existing is null) return Result.Fail(new NotFoundError());

        existing.Name = source.Name;
        await context.SaveChangesAsync();
        return Result.Ok(existing);
    }
}