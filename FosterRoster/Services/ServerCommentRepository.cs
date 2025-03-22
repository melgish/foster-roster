namespace FosterRoster.Services;

public sealed class ServerCommentRepository(
    IDbContextFactory<FosterRosterDbContext> contextFactory,
    TimeProvider timeProvider
) : ICommentRepository
{
    /// <summary>
    ///     Adds a new comment to the database.
    /// </summary>
    /// <param name="comment">Comment instance to add.</param>
    /// <returns>A Result with Comment on Success, otherwise Result with Errors.</returns>
    public async Task<Result<Comment>> AddAsync(Comment comment)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        // Workaround because of issues getting ValueGeneratedOnAdd() to work.
        comment.TimeStamp = timeProvider.GetUtcNow().UtcDateTime;
        var entry = await context.Comments.AddAsync(comment);
        await context.SaveChangesAsync();
        return Result.Ok(entry.Entity);
    }

    /// <summary>
    ///     Removes an existing comment by its primary key.
    /// </summary>
    /// <param name="commentId">ID of comment to delete.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> DeleteByKeyAsync(int commentId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context
                .Comments
                .Where(c => c.Id == commentId)
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
    /// <param name="comment">New data for the comment.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result<Comment>> UpdateAsync(int commentId, Comment comment)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var existing = await context.Comments.FirstOrDefaultAsync(e => e.Id == commentId);
        if (existing is null) return Result.Fail<Comment>(new NotFoundError());

        // Only update if comment text has actually been changed.
        if (existing.Text.Equals(comment.Text)) return Result.Ok(existing);

        existing.Text = comment.Text;
        existing.Modified = timeProvider.GetUtcNow().UtcDateTime;
        await context.SaveChangesAsync();

        return Result.Ok(existing);
    }
}