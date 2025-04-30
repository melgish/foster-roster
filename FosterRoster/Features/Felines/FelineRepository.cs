namespace FosterRoster.Features.Felines;

using Data;
using FluentResults.Extensions;
using System.Linq.Expressions;
using Fosterers;

public sealed class FelineRepository(
    IDbContextFactory<FosterRosterDbContext> dbContextFactory
)
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
                : new Fosterer
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
    public async Task<Result<FelineFormDto>> AddAsync(FelineFormDto model)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var entry = db.Felines.AddAsync(new()
        {
            AnimalId = model.AnimalId,
            Breed = model.Breed,
            Category = model.Category,
            Color = model.Color,
            FostererId = model.FostererId
        });
        await db.SaveChangesAsync();
        return Result.Ok();
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
    public async Task<Result<Feline>> GetByKeyAsync(int felineId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await db
                .Felines
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Include(f => f.Chores)
                .Include(f => f.Comments)
                .Include(f => f.Thumbnail)
                .Select(FelineProjection)
                .AsSplitQuery()
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
    /// <param name="feline">Data to assign to feline</param>
    /// <returns>A Result with Feline if updated, or errors on failure.</returns>
    public async Task<Result<Feline>> UpdateAsync(int felineId, Feline feline)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var existing = await db
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
            db.Remove(existing.Thumbnail);
            existing.Thumbnail = null;
        }
        else if (feline.Thumbnail?.ImageData.Length > 0)
        {
            existing.Thumbnail ??= new() { FelineId = existing.Id };
            existing.Thumbnail.ImageData = feline.Thumbnail.ImageData;
            existing.Thumbnail.ContentType = feline.Thumbnail.ContentType;
        }

        existing.Weaned = feline.Weaned;

        await db.SaveChangesAsync();

        // Remove thumbnail data from the returned object.
        return Result.Ok(FelineProjection.Compile().Invoke(existing));
    }
}