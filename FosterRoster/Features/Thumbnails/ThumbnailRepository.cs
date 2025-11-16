namespace FosterRoster.Features.Thumbnails;

public sealed class ThumbnailRepository(
    IDbContextFactory<Data.FosterRosterDbContext> contextFactory
) : IRepository
{
    public sealed record ThumbnailData(byte[] ImageData, string ContentType);

    public async Task<Result<ThumbnailData>> GetThumbnailAsync(int felineId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var thumbnail = await context
            .Thumbnails
            .AsNoTracking()
            .Where(t => t.FelineId == felineId)
            .Select(t => new ThumbnailData(t.ImageData, t.ContentType))
            .FirstOrDefaultAsync();
        return thumbnail is null ? Result.Fail(new NotFoundError()) : Result.Ok(thumbnail);
    }

    /// <summary>
    ///     Response for SetThumbnailAsync method.
    /// </summary>
    /// <param name="FelineId">ID of feline with updated thumbnail.</param>
    /// <param name="Version">Updated row version for the thumbnail.</param>
    public sealed record SetThumbnailResponse([UsedImplicitly] int FelineId, uint Version);

    /// <summary>
    ///     Sets the thumbnail for a feline.
    /// </summary>
    /// <param name="felineId">ID of feline to change</param>
    /// <param name="thumbnail">Thumbnail to assign to feline</param>
    /// <returns>A Result with Feline if updated, or errors on failure.</returns>
    public async Task<Result<SetThumbnailResponse>> SetThumbnailAsync(int felineId, Thumbnail thumbnail)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        // Make sure that the feline associated with this thumbnail exists.
        var feline = await context
            .Felines
            .Include(f => f.Thumbnail)
            .SingleOrDefaultAsync(e => e.Id == felineId);
        if (feline == null) return Result.Fail(new NotFoundError());

        feline.Thumbnail ??= new Thumbnail { FelineId = feline.Id };
        feline.Thumbnail.ImageData = thumbnail.ImageData;
        feline.Thumbnail.ContentType = thumbnail.ContentType;

        await context.SaveChangesAsync();

        return Result.Ok(new SetThumbnailResponse(felineId, feline.Thumbnail.Version));
    }
}