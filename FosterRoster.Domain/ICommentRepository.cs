namespace FosterRoster.Domain;

public interface ICommentRepository
{
    /// <summary>
    /// Adds a new comment to the database.
    /// </summary>
    /// <param name="comment">Comment instance to add.</param>
    /// <returns>Updated comment instance after add.</returns>
    public Task<Comment> AddAsync(Comment comment);
}