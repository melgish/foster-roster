namespace FosterRoster.Controllers;

[ApiController]
[Route("api/comments")]
public sealed class CommentsController(
    ICommentRepository commentRepository
) : ControllerBase
{
    /// <summary>
    /// Adds a new comment to the database
    /// </summary>
    /// <param name="model">Comment payload to add.</param>
    /// <returns>Updated comment after add.</returns>
    [HttpPost]
    public async Task<ActionResult<Comment>> AddAsync(CommentEditModel model)
        => await commentRepository.AddAsync(model.ToComment()) switch
        {
            { IsSuccess: true } ok => Ok(ok.Value),
            { } err => this.Unprocessable(err)
        };

    /// <summary>
    /// Deletes an existing comment by it's unique ID.
    /// </summary>
    /// <param name="commentId">ID of comment to delete.</param>
    /// <returns>True if a comment was deleted, othewrise false.</returns>
    [HttpDelete("{commentId:int}")]
    public async Task<IActionResult> DeleteByKeyAsync(int commentId)
        => await commentRepository.DeleteByKeyAsync(commentId) switch
        {
            { IsSuccess: true } => NoContent(),
            { } err => this.Unprocessable(err)
        };
}