namespace FosterRoster.Domain.Repositories;

public interface ICommentRepository: IRepository
{
    /// <summary>
    ///     Adds a new comment to the database.
    /// </summary>
    /// <param name="comment">Comment instance to add.</param>
    /// <returns>A Result with Comment on Success, otherwise Result with Errors.</returns>
    public Task<Result<Comment>> AddAsync(Comment comment);

    /// <summary>
    ///     Removes an existing comment by its primary key.
    /// </summary>
    /// <param name="commentId">ID of comment to delete.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public Task<Result> DeleteByKeyAsync(int commentId);

    /// <summary>
    ///     Update an existing comment. 
    /// </summary>
    /// <param name="commentId">ID of the comment to update.</param>
    /// <param name="comment">New data for the comment.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public Task<Result<Comment>> UpdateAsync(int commentId, Comment comment);
}