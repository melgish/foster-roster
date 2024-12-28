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
}