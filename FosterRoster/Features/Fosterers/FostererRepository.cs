namespace FosterRoster.Features.Fosterers;

public sealed class FostererRepository(
    IDbContextFactory<Data.FosterRosterDbContext> dbContextFactory
) : IRepository
{
    /// <summary>
    ///     Adds a new fosterer to the database.
    /// </summary>
    /// <param name="model">Fosterer data to add.</param>
    /// <returns>Result with Fosterer after add, or Errors on failure.</returns>
    public async Task<Result<IdOnlyDto>> AddAsync(FostererFormDto model)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var entry = db.Fosterers.Add(new Fosterer
        {
            Address = model.Address,
            ContactMethod = model.ContactMethod,
            Email = model.Email,
            Phone = model.Phone,
            Name = model.Name
        });
        await db.SaveChangesAsync();
        return Result.Ok(entry.Entity.ToIdOnly());
    }

    /// <summary>
    ///     Captures a new database context and creates a queryable for the Weight table.
    /// </summary>
    /// <returns></returns>
    public Task<Query<Fosterer>> CreateQueryAsync()
        => dbContextFactory.CreateQueryAsync(db => db.Fosterers.AsNoTracking());

    /// <summary>
    ///     Deletes a Fosterer by its ID.
    /// </summary>
    /// <param name="fostererId">ID of fosterer to remove.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> DeleteByKeyAsync(int fostererId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await db
                .Fosterers
                .Where(e => e.Id == fostererId)
                .ExecuteDeleteAsync() switch
        {
            0 => Result.Fail(new NotFoundError()),
            1 => Result.Ok(),
            _ => Result.Fail(new MultipleChangesError())
        };
    }

    /// <summary>
    ///     Gets single fosterer from the database.
    /// </summary>
    /// <param name="fostererId">ID of fosterer to return.</param>
    /// <returns>Result with Fosterer if successful, or Errors on failure.</returns>
    public async Task<Result<FostererFormDto>> GetByKeyAsync(int fostererId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var dto = await db
            .Fosterers
            .SelectToFormDto()
            .FirstOrDefaultAsync(e => e.Id == fostererId);
        return dto is null ? Result.Fail(new NotFoundError()) : Result.Ok(dto);
    }

    /// <summary>
    ///     Updates an existing Fosterer in the database.
    /// </summary>
    /// <param name="fostererId">ID of Fosterer to update</param>
    /// <param name="model">Data to assign to Fosterer</param>
    /// <returns>Result with updated Fosterer if found, or Errors on failure.</returns>
    public async Task<Result<IdOnlyDto>> UpdateAsync(int fostererId, FostererFormDto model)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var existing = await db.Fosterers.FindAsync(fostererId);
        if (existing == null) return Result.Fail(new NotFoundError());

        existing.Address = model.Address.TrimToNull();
        existing.ContactMethod = model.ContactMethod;
        existing.Email = model.Email.TrimToNull();
        existing.Phone = model.Phone.TrimToNull();
        existing.Name = model.Name.TrimToNull();
        await db.SaveChangesAsync();

        return Result.Ok(existing.ToIdOnly());
    }
}
