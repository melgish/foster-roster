namespace FosterRoster.Features.Comments;

public sealed class CommentRepository(
    IDbContextFactory<Data.FosterRosterDbContext> dbContextFactory,
    TimeProvider timeProvider
) : IRepository
{
    /// <summary>
    ///     Adds a new comment to the database.
    /// </summary>
    /// <param name="comment">Comment instance to add.</param>
    /// <returns>A Result with Comment on Success, otherwise Result with Errors.</returns>
    public async Task<Result<IdOnlyDto>> AddAsync(CommentFormDto comment)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var entry = await context.Comments.AddAsync(new Comment
        {
            FelineId = comment.FelineId,
            Text = comment.Text,
            // Workaround because of issues getting ValueGeneratedOnAdd() to work.
            TimeStamp = timeProvider.GetUtcNow().UtcDateTime
        });
        await context.SaveChangesAsync();
        return Result.Ok(entry.Entity.ToIdOnly());
    }

    public async Task<Result<Comment>> GetByKeyAsync(int commentId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var comment = await context.Comments
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == commentId);
        return comment is null
            ? Result.Fail(new NotFoundError())
            : Result.Ok(comment);
    }

    /// <summary>
    ///     Removes an existing comment by its primary key.
    /// </summary>
    /// <param name="commentId">ID of comment to delete.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> DeleteByKeyAsync(int commentId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        return await context
                .Comments
                .Where(e => e.Id == commentId)
                .ExecuteDeleteAsync() switch
        {
            0 => Result.Fail(new NotFoundError()),
            1 => Result.Ok(),
            _ => Result.Fail(new MultipleChangesError())
        };
    }

    /// <summary>
    ///     Update an existing comment.
    /// </summary>
    /// <param name="commentId">ID of the comment to update.</param>
    /// <param name="dto">New data for the comment.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result<IdOnlyDto>> UpdateAsync(int commentId, CommentFormDto dto)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var existing = await db.Comments.FindAsync(commentId);
        if (existing is null)
            return Result.Fail(new NotFoundError());

        // Only update if comment text has actually been changed.
        if (existing.Text.Equals(dto.Text, StringComparison.Ordinal))
            return Result.Ok(existing.ToIdOnly());

        existing.Text = dto.Text;
        existing.Modified = timeProvider.GetUtcNow().UtcDateTime;
        await db.SaveChangesAsync();

        return Result.Ok(existing.ToIdOnly());
    }
}
