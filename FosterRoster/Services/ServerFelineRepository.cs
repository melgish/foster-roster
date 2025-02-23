namespace FosterRoster.Services;

using System.Linq.Expressions;

public sealed class ServerFelineRepository(
    IDbContextFactory<FosterRosterDbContext> contextFactory
) : IFelineRepository
{
    /// <summary>
    ///     Projection to select only the fields needed for the feline list.
    /// </summary>
    private static readonly Expression<Func<Feline, Feline>> FelineProjection =
        f => new()
        {
            Id = f.Id,
            AnimalId = f.AnimalId,
            Breed = f.Breed,
            Category = f.Category,
            Color = f.Color,
            Comments = f.Comments.OrderByDescending(c => c.TimeStamp).ToList(),
            // Just include the name for grid layout.
            Fosterer = f.Fosterer == null
                ? null
                : new Fosterer()
                {
                    Id = f.Fosterer!.Id,
                    Name = f.Fosterer!.Name
                },
            FostererId = f.FostererId,
            Gender = f.Gender,
            IntakeAgeInWeeks = f.IntakeAgeInWeeks,
            IntakeDate = f.IntakeDate,
            Name = f.Name,
            RegistrationDate = f.RegistrationDate,
            SourceId = f.SourceId,
            // Don't include the file data, just enough to generate the thumbnail URL
            Thumbnail = f.Thumbnail == null
                ? null
                : new()
                {
                    FelineId = f.Thumbnail.FelineId,
                    ContentType = f.Thumbnail.ContentType,
                    Version = f.Thumbnail.Version
                },
            Weaned = f.Weaned,
            // Only include 7 days of weights.
            Weights = f.Weights
                .OrderByDescending(w => w.DateTime)
                .Take(7)
                .ToList(),
            InactivatedAtUtc = f.InactivatedAtUtc,
            IsInactive = f.IsInactive
        };

    /// <summary>
    ///     Restores identified feline to active status.
    /// </summary>
    /// <param name="felineId">ID of feline to update.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> ActivateAsync(int felineId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
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
    /// <param name="feline">Feline instance to add.</param>
    /// <returns>A Result with added feline, or errors on failure.</returns>
    public async Task<Result<FelineEditModel>> AddAsync(FelineEditModel feline)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var entry = await context.Felines.AddAsync(feline.ToFeline());
        await context.SaveChangesAsync();
        return Result.Ok(new FelineEditModel(FelineProjection.Compile().Invoke(entry.Entity)));
    }

    /// <summary>
    ///     Sets a feline as inactive in the database.
    /// </summary>
    /// <param name="felineId">ID of feline to deactivate.</param>
    /// <param name="dateTimeUtc">Date and Time of deactivation.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> DeactivateAsync(int felineId, DateTimeOffset dateTimeUtc)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context
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
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context
                .Felines
                .Where(f => f.Id == felineId)
                .ExecuteDeleteAsync() switch
            {
                0 => Result.Fail(new NotFoundError()),
                1 => Result.Ok(),
                _ => Result.Fail(new MultipleChangesError())
            };
    }

    /// <summary>
    ///     Get list of all felines in the database.
    /// </summary>
    /// <returns>A Result with list of felines, or errors on failure.</returns>
    public async Task<Result<List<Feline>>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return Result.Ok(
            await context
                .Felines
                .AsNoTracking()
                .Include(f => f.Thumbnail)
                .OrderBy(f => f.Name)
                .Select(FelineProjection)
                .AsSplitQuery()
                .ToListAsync()
        );
    }

    /// <summary>
    ///     Get list of all felines in the database, with only their names and ids.
    /// </summary>
    /// <returns>A Result with list of items, or errors on failure.</returns>
    public async Task<Result<List<ListItem<int>>>> GetAllNamesAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return Result.Ok(
            await context
                .Felines
                .AsNoTracking()
                .OrderBy(f => f.Name)
                .Select(f => new ListItem<int>(f.Id, f.Name))
                .ToListAsync()
        );
    }

    /// <summary>
    ///     Gets a single feline by ID.
    /// </summary>
    /// <param name="felineId">ID of feline to get.</param>
    /// <returns>A Result with Feline if found, or errors on failure</returns>
    public async Task<Result<Feline>> GetByKeyAsync(int felineId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context
                .Felines
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Include(f => f.Thumbnail)
                .Include(f => f.Comments)
                .Where(f => f.Id == felineId)
                .Select(FelineProjection)
                .AsSplitQuery()
                .SingleOrDefaultAsync() switch
            {
                null => Result.Fail(new NotFoundError()),
                { } feline => Result.Ok(feline)
            };
    }

    /// <summary>
    ///     Gets the thumbnail for a single feline.
    /// </summary>
    /// <param name="felineId">ID of the feline</param>
    /// <returns>A Result with Thumbnail if found, or errors on failure.</returns>
    public async Task<Result<Thumbnail>> GetThumbnailAsync(int felineId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context
                .Thumbnails
                .AsNoTracking()
                .Where(t => t.FelineId == felineId)
                .SingleOrDefaultAsync() switch
            {
                null => Result.Fail(new NotFoundError()),
                { } thumbnail => Result.Ok(thumbnail)
            };
    }

    
    
    /// <summary>
    ///     Sets the thumbnail for a feline.
    /// </summary>
    /// <param name="felineId">ID of feline to change</param>
    /// <param name="thumbnail">Thumbnail to assign to feline</param>
    /// <returns>A Result with Feline if updated, or errors on failure.</returns>
    public async Task<Result<SetThumbnailResponse>> SetThumbnailAsync(int felineId, Thumbnail thumbnail)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        var feline = await context
            .Felines
            .Include(f => f.Thumbnail)
            .SingleOrDefaultAsync(e => e.Id == felineId);
        if (feline == null) return Result.Fail(new NotFoundError());

        feline.Thumbnail ??= new() { FelineId = feline.Id };
        feline.Thumbnail.ImageData = thumbnail.ImageData;
        feline.Thumbnail.ContentType = thumbnail.ContentType;

        await context.SaveChangesAsync();

        return Result.Ok(new SetThumbnailResponse(feline.Id, feline.Thumbnail.Version));
    }

    /// <summary>
    ///     Updates a feline in the database.
    /// </summary>
    /// <param name="felineId">ID of feline to update</param>
    /// <param name="feline">Data to assign to feline</param>
    /// <returns>A Result with Feline if updated, or errors on failure.</returns>
    public async Task<Result<Feline>> UpdateAsync(int felineId, Feline feline)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var existing = await context
            .Felines
            .Include(f => f.Thumbnail)
            .SingleOrDefaultAsync(e => e.Id == felineId);
        if (existing == null) return Result.Fail(new NotFoundError());

        existing.AnimalId = feline.AnimalId;
        existing.Breed = feline.Breed;
        existing.Category = feline.Category;
        existing.Color = feline.Color;
        existing.FostererId = feline.FostererId;
        existing.Gender = feline.Gender;
        existing.IntakeAgeInWeeks = feline.IntakeAgeInWeeks;
        existing.IntakeDate = feline.IntakeDate;
        existing.Name = feline.Name;
        existing.RegistrationDate = feline.RegistrationDate;
        existing.SourceId = feline.SourceId;
        if (existing.Thumbnail is not null && feline.Thumbnail is null)
        {
            context.Remove(existing.Thumbnail);
            existing.Thumbnail = null;
        }
        else if (feline.Thumbnail?.ImageData.Length > 0)
        {
            existing.Thumbnail ??= new() { FelineId = existing.Id };
            existing.Thumbnail.ImageData = feline.Thumbnail.ImageData;
            existing.Thumbnail.ContentType = feline.Thumbnail.ContentType;
        }

        existing.Weaned = feline.Weaned;

        await context.SaveChangesAsync();

        // Remove thumbnail data from the returned object.
        return Result.Ok(FelineProjection.Compile().Invoke(existing));
    }
}