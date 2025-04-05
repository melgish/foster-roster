namespace FosterRoster.Services;

public sealed class ServerChoresRepository(
    IDbContextFactory<FosterRosterDbContext> dbContextFactory
) : IChoresRepository
{
    /// <summary>
    ///     Adds a new chore to the database.
    /// </summary>
    /// <param name="chore">Chore instance to add.</param>
    /// <returns>A Result with Chore on Success, otherwise Result with Errors.</returns>
    public async Task<Result<Chore>> AddAsync(Chore chore)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var entry = await context.Chores.AddAsync(chore);
        await context.SaveChangesAsync();
        return Result.Ok(entry.Entity);
    }
    
    /// <summary>
    ///     Deletes a Chore by its ID.
    /// </summary>
    /// <param name="choreId">ID of chore to remove.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> DeleteByKeyAsync(int choreId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        return await context.Chores.Where(s => s.Id == choreId).ExecuteDeleteAsync() switch
        {
            0 => Result.Fail(new NotFoundError()),
            1 => Result.Ok(),
            _ => Result.Fail(new MultipleChangesError())
        };
    }

    /// <summary>
    ///     Updates an existing Chore in the database.
    /// </summary>
    /// <param name="choreId">ID of chore to update.</param>
    /// <param name="chore">Data to assign to Chore</param>
    /// <returns>Result with updated Chore if found, or Errors on failure.</returns>
    public async Task<Result<Chore>> UpdateAsync(int choreId, Chore chore)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var existing = await context.Chores.FindAsync(choreId);
        if (existing is null) return Result.Fail(new NotFoundError());

        existing.Description = chore.Description;
        existing.Name = chore.Name;
        
        await context.SaveChangesAsync();
        return Result.Ok(existing);
    }

}