namespace FosterRoster.Features.Microchips;

using Data;

public sealed class MicrochipsRepository(
    IDbContextFactory<FosterRosterDbContext> dbContextFactory
) : IRepository
{
    /// <summary>
    ///     Add a new microchip to the database.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<Result<IdOnlyDto>> AddAsync(MicrochipFormDto dto)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var entry = db.Microchips.Add(new Microchip
        {
            Brand = dto.Brand.Trim(),
            Code = dto.Code.Trim(),
            Comment = dto.Comment.TrimToNull(),
            FelineId = dto.FelineId
        });
        await db.SaveChangesAsync();
        return Result.Ok(entry.Entity.ToIdOnly());
    }

    /// <summary>
    ///     Deletes an existing Microchip from the database by its ID.
    /// </summary>
    /// <param name="microchipId">ID of microchip to remove.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> DeleteByKeyAsync(int microchipId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await db
                .Microchips
                .Where(e => e.Id == microchipId)
                .ExecuteDeleteAsync() switch
        {
            0 => Result.Fail(new NotFoundError()),
            1 => Result.Ok(),
            _ => Result.Fail(new MultipleChangesError())
        };
    }

    /// <summary>
    ///     Gets a microchip for the supplied Feline from the database.
    /// </summary>
    /// <param name="felineId">ID of vaccination to fetch.</param>
    /// <returns>Result with Vaccinations if successful, or Errors on failure.</returns>
    public async Task<Result<MicrochipFormDto>> GetByFelineIdAsync(int felineId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var model = await db
            .Microchips
            .SelectToFormDto()
            .FirstOrDefaultAsync(e => e.FelineId == felineId);
        return model is null ? Result.Fail(new NotFoundError()) : Result.Ok(model);
    }

    /// <summary>
    ///     Updates an existing Microchip in the database.
    /// </summary>
    /// <param name="microchipId">ID of vaccination to update.</param>
    /// <param name="dto">Data to assign to Vaccinations</param>
    /// <returns>Result with updated Vaccinations if found, or Errors on failure.</returns>
    public async Task<Result<IdOnlyDto>> UpdateAsync(int microchipId, MicrochipFormDto dto)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();

        var existing = await db.Microchips.FindAsync(microchipId);
        if (existing is null)
            return Result.Fail(new NotFoundError());

        existing.Brand = dto.Brand.Trim();
        existing.Code = dto.Code.Trim();
        existing.Comment = dto.Comment?.TrimToNull();

        await db.SaveChangesAsync();

        return Result.Ok(existing.ToIdOnly());
    }
}