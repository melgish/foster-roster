namespace FosterRoster.Features.Felines;

public sealed class FelineRepository(
    IDbContextFactory<Data.FosterRosterDbContext> dbContextFactory
)
{
    /// <summary>
    ///     Restores identified feline to active status.
    /// </summary>
    /// <param name="felineId">ID of feline to update.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> ActivateAsync(int felineId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        return await context
                .Felines
                .IgnoreQueryFilters()
                .Where(f => f.Id == felineId && f.IsInactive)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(f => f.IsInactive, false)
                    .SetProperty(f => f.InactivatedAtUtc, default(DateTimeOffset?))
                ) switch
            {
                0 => Result.Fail(new NoChangesError()),
                1 => Result.Ok(),
                _ => Result.Fail(new MultipleChangesError())
            };
    }

    /// <summary>
    ///     Adds a new feline to the database.
    /// </summary>
    /// <param name="model">Feline instance to add.</param>
    /// <returns>A Result with added feline, or errors on failure.</returns>
    public async Task<Result<IdOnlyDto>> AddAsync(FelineFormDto model)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var entry = db.Felines.Add(new()
        {
            AnimalId = model.AnimalId,
            Breed = model.Breed,
            Category = model.Category,
            Color = model.Color,
            FostererId = model.FostererId,
            Gender = model.Gender,
            IntakeAgeInWeeks = model.IntakeAgeInWeeks,
            IntakeDate = model.IntakeDate.GetValueOrDefault(),
            Name = model.Name,
            SourceId = model.SourceId,
            SterilizationDate = model.SterilizationDate,
            Weaned = model.Weaned,

            Thumbnail = model.Thumbnail
        });
        await db.SaveChangesAsync();
        return Result.Ok(new IdOnlyDto(entry.Entity.Id));
    }

    /// <summary>
    ///     Captures a new database context and creates a queryable for the Feline table.
    /// </summary>
    /// <returns></returns>
    public Task<Query<Feline>> CreateQueryAsync()
        => dbContextFactory.CreateQueryAsync(db => db.Felines.AsNoTracking());

    /// <summary>
    ///     Sets a feline as inactive in the database.
    /// </summary>
    /// <param name="felineId">ID of feline to deactivate.</param>
    /// <param name="dateTimeUtc">Date and Time of deactivation.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> DeactivateAsync(int felineId, DateTimeOffset dateTimeUtc)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await db
                .Felines
                .Where(f => f.Id == felineId)
                .ExecuteUpdateAsync(f => f
                    // Postgres needs +00:00
                    .SetProperty(p => p.InactivatedAtUtc, dateTimeUtc.ToUniversalTime())
                    .SetProperty(p => p.IsInactive, true)
                ) switch
            {
                0 => Result.Fail(new NoChangesError()),
                1 => Result.Ok(),
                _ => Result.Fail(new MultipleChangesError())
            };
    }

    /// <summary>
    ///     Deletes a feline by its ID.
    /// </summary>
    /// <param name="felineId">ID of feline to remove.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> DeleteByKeyAsync(int felineId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await db.Felines
                .Where(e => e.Id == felineId)
                .ExecuteDeleteAsync() switch
            {
                0 => Result.Fail(new NotFoundError()),
                1 => Result.Ok(),
                _ => Result.Fail(new MultipleChangesError())
            };
    }

    /// <summary>
    ///     Gets a single feline by ID.
    /// </summary>
    /// <param name="felineId">ID of feline to get.</param>
    /// <returns>A Result with Feline if found, or errors on failure</returns>
    public async Task<Result<FelineFormDto>> GetByKeyAsync(int felineId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await db
                .Felines
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Include(f => f.Chores)
                .Include(f => f.Thumbnail)
                .SelectToFormDto()
                .SingleOrDefaultAsync(f => f.Id == felineId) switch
            {
                null => Result.Fail(new NotFoundError()),
                { } feline => Result.Ok(feline)
            };
    }

    /// <summary>
    ///     Updates a feline in the database.
    /// </summary>
    /// <param name="felineId">ID of feline to update</param>
    /// <param name="model">Data to assign to feline</param>
    /// <returns>A Result with Feline if updated, or errors on failure.</returns>
    public async Task<Result<IdOnlyDto>> UpdateAsync(int felineId, FelineFormDto model)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var existing = await db
            .Felines
            .Include(f => f.Thumbnail)
            .SingleOrDefaultAsync(e => e.Id == felineId);
        if (existing == null) return Result.Fail(new NotFoundError());

        existing.AnimalId = model.AnimalId;
        existing.Breed = model.Breed;
        existing.Category = model.Category;
        existing.Color = model.Color;
        existing.FostererId = model.FostererId;
        existing.Gender = model.Gender;
        existing.IntakeDate = model.IntakeDate ?? existing.IntakeDate;
        existing.IntakeAgeInWeeks = model.IntakeAgeInWeeks;

        existing.Name = model.Name;
        existing.SourceId = model.SourceId;
        existing.SterilizationDate = model.SterilizationDate;
        if (existing.Thumbnail is not null && model.Thumbnail is null)
        {
            db.Remove(existing.Thumbnail);
            existing.Thumbnail = null;
        }
        else if (model.Thumbnail?.ImageData.Length > 0)
        {
            existing.Thumbnail ??= new() { FelineId = existing.Id };
            existing.Thumbnail.ImageData = model.Thumbnail.ImageData;
            existing.Thumbnail.ContentType = model.Thumbnail.ContentType;
        }

        existing.Weaned = model.Weaned;

        await db.SaveChangesAsync();

        return Result.Ok(new IdOnlyDto(existing.Id));
    }
}