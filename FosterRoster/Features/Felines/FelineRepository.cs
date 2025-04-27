namespace FosterRoster.Features.Felines;

using Data;
using FluentResults.Extensions;
using System.Linq.Expressions;
using Fosterers;

public sealed class FelineRepository(
    IDbContextFactory<FosterRosterDbContext> contextFactory
)
{
    private static DbSet<Feline> SetFactory(FosterRosterDbContext db) => db.Felines;

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
    /// <param name="model">Feline instance to add.</param>
    /// <returns>A Result with added feline, or errors on failure.</returns>
    public Task<Result<FelineFormDto>> AddAsync(FelineFormDto model)
        => contextFactory
            .AddAsync(SetFactory, model.ToFeline())
            .Map(feline => new FelineFormDto(FelineProjection.Compile().Invoke(feline)));

    /// <summary>
    ///     Captures a new database context and creates a queryable for the Feline table.
    /// </summary>
    /// <returns></returns>
    public Task<Query<Feline>> CreateQueryAsync() => contextFactory.CreateQueryAsync(SetFactory);

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
    public Task<Result> DeleteByKeyAsync(int felineId)
        => contextFactory.DeleteByKeyAsync(SetFactory, felineId);

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
                .Include(f => f.Chores)
                .Include(f => f.Comments)
                .Include(f => f.Thumbnail)
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