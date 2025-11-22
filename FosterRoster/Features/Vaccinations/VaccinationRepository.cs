using FosterRoster.Data;

namespace FosterRoster.Features.Vaccinations;

public class VaccinationRepository(
    IDbContextFactory<FosterRosterDbContext> dbContextFactory
) : IRepository
{
    public async Task<Result<IdOnlyDto>> AddAsync(VaccinationFormDto dto)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        db.Vaccinations.AddRange(
            dto.FelineIds.Select(felineId => new Vaccination
            {
                AdministeredBy = dto.AdministeredBy.Trim(),
                Comments = dto.Comments?.TrimToNull(),
                ExpirationDate = dto.ExpirationDate,
                FelineId = felineId,
                ManufacturerName = dto.ManufacturerName.Trim(),
                SerialNumber = dto.SerialNumber.TrimToNull(),
                VaccinationDate = dto.VaccinationDate!.Value,
                VaccineName = dto.VaccineName.Trim()
            }));
        await db.SaveChangesAsync();
        return Result.Ok(IdOnly.Zero);
    }

    /// <summary>
    ///     Captures a new database context and creates a queryable for the Vaccination table.
    /// </summary>
    /// <returns></returns>
    public Task<Query<Vaccination>> CreateQueryAsync()
        => dbContextFactory.CreateQueryAsync(db => db.Vaccinations.AsNoTracking());

    /// <summary>
    ///     Deletes an existing Vaccination from the database by its ID.
    /// </summary>
    /// <param name="vaccinationId">ID of vaccination to remove.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> DeleteByKeyAsync(int vaccinationId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await db
                .Vaccinations
                .Where(e => e.Id == vaccinationId)
                .ExecuteDeleteAsync() switch
            {
                0 => Result.Fail(new NotFoundError()),
                1 => Result.Ok(),
                _ => Result.Fail(new MultipleChangesError())
            };
    }

    /// <summary>
    ///     Gets an existing Vaccinations from the database by its ID.
    /// </summary>
    /// <param name="vaccinationId">ID of vaccination to fetch.</param>
    /// <returns>Result with Vaccinations if successful, or Errors on failure.</returns>
    public async Task<Result<VaccinationFormDto>> GetByKeyAsync(int vaccinationId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var model = await db
            .Vaccinations
            .SelectToFormDto()
            .FirstOrDefaultAsync(e => e.Id == vaccinationId);
        return model is null ? Result.Fail(new NotFoundError()) : Result.Ok(model);
    }

    /// <summary>
    ///     Updates an existing Vaccinations in the database.
    /// </summary>
    /// <param name="vaccinationId">ID of vaccination to update.</param>
    /// <param name="model">Data to assign to Vaccinations</param>
    /// <returns>Result with updated Vaccinations if found, or Errors on failure.</returns>
    public async Task<Result<IdOnlyDto>> UpdateAsync(int vaccinationId, VaccinationFormDto model)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();

        var existing = await db.Vaccinations.FindAsync(vaccinationId);
        if (existing is null)
            return Result.Fail(new NotFoundError());

        existing.AdministeredBy = model.AdministeredBy.Trim();
        existing.Comments = model.Comments?.TrimToNull();
        existing.ExpirationDate = model.ExpirationDate;
        existing.FelineId = model.FelineId;
        existing.ManufacturerName = model.ManufacturerName.Trim();
        existing.SerialNumber = model.SerialNumber?.TrimToNull();
        existing.VaccinationDate = model.VaccinationDate!.Value;
        existing.VaccineName = model.VaccineName.Trim();

        await db.SaveChangesAsync();

        return Result.Ok(existing.ToIdOnly());
    }
}