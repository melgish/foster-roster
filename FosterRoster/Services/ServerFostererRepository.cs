namespace FosterRoster.Services;

public sealed class ServerFostererRepository(
    IDbContextFactory<FosterRosterDbContext> contextFactory
) : IFostererRepository
{
    /// <summary>
    ///     Adds a new fosterer to the database.
    /// </summary>
    /// <param name="fosterer">Fosterer instance to add.</param>
    /// <returns>Result with Fosterer after add, or Errors on failure.</returns>
    public async Task<Result<Fosterer>> AddAsync(Fosterer fosterer)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var entry = await context.Fosterers.AddAsync(fosterer);
        await context.SaveChangesAsync();
        return Result.Ok(entry.Entity);
    }

    /// <summary>
    ///     Deletes a Fosterer by its ID.
    /// </summary>
    /// <param name="fostererId">ID of fosterer to remove.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> DeleteByKeyAsync(int fostererId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context
                .Fosterers
                .Where(f => f.Id == fostererId)
                .ExecuteDeleteAsync() switch
            {
                0 => Result.Fail(new NotFoundError()),
                1 => Result.Ok(),
                _ => Result.Fail(new MultipleChangesError())
            };
    }

    /// <summary>
    ///     Gets list of all fosterers from the database.
    /// </summary>
    /// <returns>Result with list of fosterers, or Errors on failure.</returns>
    public async Task<Result<List<Fosterer>>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return Result.Ok(
            await context
                .Fosterers
                .AsNoTracking()
                .OrderBy(f => f.Name)
                .ToListAsync()
        );
    }

    /// <summary>
    ///     Gets single fosterer from the database.
    /// </summary>
    /// <param name="fostererId">ID of fosterer to return.</param>
    /// <returns>Result with Fosterer if successful, or Errors on failure.</returns>
    public async Task<Result<Fosterer>> GetByKeyAsync(int fostererId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context
                .Fosterers
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == fostererId) switch
            {
                null => Result.Fail(new NotFoundError()),
                { } fosterer => Result.Ok(fosterer)
            };
    }

    /// <summary>
    ///     Updates an existing Fosterer in the database.
    /// </summary>
    /// <param name="fostererId">ID of Fosterer to update</param>
    /// <param name="fosterer">Data to assign to Fosterer</param>
    /// <returns>Result with updated Fosterer if found, or Errors on failure.</returns>
    public async Task<Result<Fosterer>> UpdateAsync(int fostererId, Fosterer fosterer)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var existing = await context
            .Fosterers
            .FirstOrDefaultAsync(e => e.Id == fostererId);
        if (existing is null) return Result.Fail(new NotFoundError());

        existing.Address = fosterer.Address;
        existing.ContactMethod = fosterer.ContactMethod;
        existing.Email = fosterer.Email;
        existing.Phone = fosterer.Phone;
        existing.Name = fosterer.Name;

        await context.SaveChangesAsync();

        return Result.Ok(existing);
    }
}