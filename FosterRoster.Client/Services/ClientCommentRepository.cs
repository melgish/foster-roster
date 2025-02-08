namespace FosterRoster.Client.Services;

public sealed class ClientCommentRepository(
    HttpClient httpClient
) : ICommentRepository
{
    private const string Route = "api/comments";
    private const string FailedToCreate = "Failed to create comment";
    private const string FailedToDelete = "Failed to delete comment";
    private const string FailedToUpdate = "Failed to update comment";

    /// <summary>
    ///     Adds a new comment to the database.
    /// </summary>
    /// <param name="comment">Comment instance to add.</param>
    /// <returns>A Result with Comment on Success, otherwise Result with Errors.</returns>
    public async Task<Result<Comment>> AddAsync(Comment comment)
        => await Result
            .Try(() => httpClient.PostAsJsonAsync<CommentEditModel>(Route, new(comment)))
            .Bind(rs => Result.OkIf(rs.IsSuccessStatusCode, FailedToCreate).ToResult(rs))
            .Bind(rs => Result.Try(() => rs.Content.ReadFromJsonAsync<Comment>()))
            .Bind(c => Result.OkIf(c is not null, FailedToCreate).ToResult(c!));

    /// <summary>
    ///     Removes an existing comment by its primary key.
    /// </summary>
    /// <param name="commentId">ID of comment to delete.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> DeleteByKeyAsync(int commentId)
        => await Result
            .Try(() => httpClient.DeleteAsync($"{Route}/{commentId}"))
            .Bind(rs => Result.OkIf(rs.IsSuccessStatusCode, FailedToDelete));

    /// <summary>
    ///     Update an existing comment. 
    /// </summary>
    /// <param name="commentId">ID of the comment to update.</param>
    /// <param name="comment">New data for the comment.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result<Comment>> UpdateAsync(int commentId, Comment comment)
        => await Result
            .Try(() => httpClient.PutAsJsonAsync($"{Route}/{commentId}", new CommentEditModel(comment)))
            .Bind(rs => Result.OkIf(rs.IsSuccessStatusCode, FailedToUpdate).ToResult(rs))
            .Bind(rs => Result.Try(() => rs.Content.ReadFromJsonAsync<Comment>()))
            .Bind(c => Result.OkIf(c is not null, FailedToUpdate).ToResult(c!));
}