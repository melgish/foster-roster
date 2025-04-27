namespace FosterRoster.Features.Fosterers;

using Data;

public sealed class FostererRepository(
    IDbContextFactory<FosterRosterDbContext> dbContextFactory
)
{
    private static DbSet<Fosterer> SetFactory(FosterRosterDbContext db) => db.Fosterers;

    /// <summary>
    ///     Adds a new fosterer to the database.
    /// </summary>
    /// <param name="fosterer">Fosterer instance to add.</param>
    /// <returns>Result with Fosterer after add, or Errors on failure.</returns>
    public Task<Result<Fosterer>> AddAsync(Fosterer fosterer)
        => dbContextFactory.AddAsync(SetFactory, fosterer);

    /// <summary>
    ///     Captures a new database context and creates a queryable for the Weight table.
    /// </summary>
    /// <returns></returns>
    public Task<Query<Fosterer>> CreateQueryAsync()
        => dbContextFactory.CreateQueryAsync(SetFactory);

    /// <summary>
    ///     Deletes a Fosterer by its ID.
    /// </summary>
    /// <param name="fostererId">ID of fosterer to remove.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public Task<Result> DeleteByKeyAsync(int fostererId)
        => dbContextFactory.DeleteByKeyAsync(SetFactory, fostererId);

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
    /// <param name="fosterer">Data to assign to Fosterer</param>
    /// <returns>Result with updated Fosterer if found, or Errors on failure.</returns>
    public Task<Result<Fosterer>> UpdateAsync(int fostererId, Fosterer fosterer)
        => dbContextFactory.UpdateAsync(SetFactory, fostererId,
            existing =>
            {
                existing.Address = fosterer.Address;
                existing.ContactMethod = fosterer.ContactMethod;
                existing.Email = fosterer.Email;
                existing.Phone = fosterer.Phone;
                existing.Name = fosterer.Name;
                return true;
            });
}