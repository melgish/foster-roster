namespace FosterRoster.Services;

using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using FosterRoster.Data;
using FosterRoster.Domain;

using Microsoft.EntityFrameworkCore;

public sealed class ServerFelineRepository(
    IDbContextFactory<FosterRosterDbContext> contextFactory
) : IFelineRepository
{
    /// <summary>
    /// Projection to select only the fields needed for the feline list.
    /// </summary>
    private static readonly Expression<Func<Feline, Feline>> FelineProjection =
        f => new()
        {
            Id = f.Id,
            Breed = f.Breed,
            Category = f.Category,
            Gender = f.Gender,
            IntakeAgeInWeeks = f.IntakeAgeInWeeks,
            IntakeDate = f.IntakeDate,
            Name = f.Name,
            RegistrationDate = f.RegistrationDate,
            // Don't include the data, just enough to generate the thumbnail URL
            Thumbnail = f.Thumbnail == null ? null : new()
            {
                FelineId = f.Thumbnail.FelineId,
                ContentType = f.Thumbnail.ContentType,
                Version = f.Thumbnail.Version,
            },
            Weaned = f.Weaned,
            Weights = f.Weights.Take(7).ToList(),
        };

    /// <summary>
    /// Adds a new cat to the database.
    /// </summary>
    /// <param name="feline">Feline instance to add.</param>
    /// <returns>Updated feline instance after add.</returns>
    public async Task<Feline> AddAsync(Feline feline)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var entry = await context.Felines.AddAsync(feline);
        await context.SaveChangesAsync();
        return entry.Entity;
    }

    /// <summary>
    /// Deletes a cat by it's id.
    /// </summary>
    /// <param name="felineId">Id of feline to remove.</param>
    /// <returns>True if a cat was removed otherwise false.</returns>
    public async Task<bool> DeleteByKeyAsync(int felineId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context
            .Felines
            .Where(f => f.Id == felineId)
            .ExecuteDeleteAsync() > 0;
    }

    /// <summary>
    /// Get list of all cats in the database.
    /// </summary>
    /// <returns>List of cats, or empty list if no cats exist.</returns>
    public async Task<List<Feline>> GetAllAsync()
    {
        await using var context = contextFactory.CreateDbContext();
        return await context
            .Felines
            .Include(f => f.Thumbnail)
            .OrderBy(f => f.Name)
            .Select(FelineProjection)
            .ToListAsync();
    }

    public async Task<List<ListItem<int>>> GetAllNamesAsync()
    {
        await using var context = contextFactory.CreateDbContext();
        return await context
            .Felines
            .OrderBy(f => f.Name)
            .Select(f => new ListItem<int>(f.Id, f.Name))
            .ToListAsync();
    }

    /// <summary>
    /// Gets a single cat by ID.
    /// </summary>
    /// <param name="felineId"></param>
    /// <returns>A single cat if found, otherwise null</returns>
    public async Task<Feline?> GetByIdAsync(int felineId)
    {
        await using var context = contextFactory.CreateDbContext();
        return await context
            .Felines
            .Include(f => f.Thumbnail)
            .Where(f => f.Id == felineId)
            .Select(FelineProjection)
            .SingleOrDefaultAsync();
    }

    /// <summary>
    /// Gets the thumbnail for a single cat.
    /// </summary>
    /// <param name="felineId">Id of the cat</param>
    /// <returns>Thumbnail if found, otherwise null</returns>
    public async Task<Thumbnail?> GetThumbnailAsync(int felineId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context
            .Thumbnails
            .Where(t => t.FelineId == felineId)
            .SingleOrDefaultAsync();
    }

    /// <summary>
    /// Sets the thumbnail for a cat.
    /// </summary>
    /// <param name="felineId">Id of cat to change</param>
    /// <param name="thumbnail">Thumbnail to assign to cat</param>
    /// <returns>Updated cat, or null if cat was not found</returns>
    public async Task<Feline?> SetThumbnailAsync(int felineId, Thumbnail thumbnail)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var feline = await context
            .Felines
            .Include(f => f.Thumbnail)
            .SingleOrDefaultAsync(e => e.Id == felineId);
        if (feline == null)
        {
            return null;
        }
        feline.Thumbnail ??= new() { FelineId = feline.Id };
        feline.Thumbnail.ImageData = thumbnail.ImageData;
        feline.Thumbnail.ContentType = thumbnail.ContentType;
        await context.SaveChangesAsync();

        return feline;
    }

    /// <summary>
    /// Updates a cat in the database.
    /// </summary>
    /// <param name="felineId">Id of cat to update</param>
    /// <param name="feline">Data to assign to cat</param>
    /// <returns>Updated cat if found, otherwise null</returns>
    public async Task<Feline?> UpdateAsync(int felineId, Feline feline)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var existing = await context
            .Felines
            .Include(f => f.Thumbnail)
            .SingleOrDefaultAsync(e => e.Id == felineId);
        if (existing == null)
        {
            return null;
        }

        existing.Breed = feline.Breed;
        existing.Category = feline.Category;
        existing.Gender = feline.Gender;
        existing.IntakeAgeInWeeks = feline.IntakeAgeInWeeks;
        existing.IntakeDate = feline.IntakeDate;
        existing.Name = feline.Name;
        existing.RegistrationDate = feline.RegistrationDate;
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
        return FelineProjection.Compile().Invoke(existing);
    }
}