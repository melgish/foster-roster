namespace FosterRoster.Features.Sources;

using Data;

public sealed class SourceRepository(
    IDbContextFactory<FosterRosterDbContext> dbContextFactory
)
{
    /// <summary>
    ///     Adds a new Source to the database.
    /// </summary>
    /// <param name="dto">Source to add</param>
    /// <returns>A Result with Updated Dto if successful, or Errors on failure.</returns>
    public async Task<Result<SourceFormDto>> AddAsync(SourceFormDto dto)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var entry = db.Sources.Add(new()
        {
            Name = dto.Name.TrimToNull()
        });
        await db.SaveChangesAsync();
        return Result.Ok(entry.Entity.ToFormDto());
    }

    /// <summary>
    ///     Captures a new database context and creates a queryable for the Source table.
    /// </summary>
    /// <returns></returns>
    public Task<Query<Source>> CreateQueryAsync()
        => dbContextFactory.CreateQueryAsync(db => db.Sources.AsNoTracking());

    /// <summary>
    ///     Deletes an existing Source from the database by its ID.
    /// </summary>
    /// <param name="sourceId">ID of source to remove.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> DeleteByKeyAsync(int sourceId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await db
                .Sources
                .Where(e => e.Id == sourceId)
                .ExecuteDeleteAsync() switch
            {
                0 => Result.Fail(new NotFoundError()),
                1 => Result.Ok(),
                _ => Result.Fail(new MultipleChangesError())
            };
    }

    /// <summary>
    ///     Gets an existing Source from the database by its ID.
    /// </summary>
    /// <param name="sourceId">ID of source to fetch.</param>
    /// <returns>Result with Source if successful, or Errors on failure.</returns>
    public async Task<Result<SourceFormDto>> GetByKeyAsync(int sourceId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var dto = await db
            .Sources
            .SelectToFormDto()
            .FirstOrDefaultAsync(e => e.Id == sourceId);
        return (dto is null) ? Result.Fail(new NotFoundError()) : Result.Ok(dto);
    }

    /// <summary>
    ///     Updates an existing Source in the database.
    /// </summary>
    /// <param name="sourceId">ID of source to update.</param>
    /// <param name="dto">Data to assign to Source</param>
    /// <returns>Result with updated Source if found, or Errors on failure.</returns>
    public async Task<Result<SourceFormDto>> UpdateAsync(int sourceId, SourceFormDto dto)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var source = await db.Sources.FindAsync(sourceId);
        if (source is null)
            return Result.Fail(new NotFoundError());

        source.Name = dto.Name.TrimToNull();
        
        await db.SaveChangesAsync();
        return Result.Ok(source.ToFormDto());
    }
}