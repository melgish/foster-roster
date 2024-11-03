namespace FosterRoster.Controllers;

[ApiController]
[Route("api/comments")]
public sealed class CommentsController(
    IValidator<CommentEditModel> commentEditModelValidator,
    ICommentRepository commentRepository
) : ControllerBase
{
    /// <summary>
    /// Adds a new comment to the database
    /// </summary>
    /// <param name="model">Comment payload to add.</param>
    /// <returns>Updated comment after add.</returns>
    [HttpPost]
    public async Task<Comment> AddAsync(CommentEditModel model)
    {
        await commentEditModelValidator.ValidateAndThrowAsync(model);
        return await commentRepository.AddAsync(model.ToComment());
    }

    /// <summary>
    /// Deletes an existing comment by it's unique ID.
    /// </summary>
    /// <param name="commentId">ID of comment to delete.</param>
    /// <returns>True if a comment was deleted, othewrise false.</returns>
    [HttpDelete("{commentId:int}")]
    public async Task<bool> DeleteByKeyAsync(int commentId)
        => await commentRepository.DeleteByKeyAsync(commentId);
}