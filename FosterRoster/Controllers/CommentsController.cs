namespace FosterRoster.Controllers;

using FluentValidation;
using FosterRoster.Domain;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/comments")]
public sealed class CommentsController(
    IValidator<CommentEditModel> commentEditModelValidator,
    ICommentRepository commentRepository
) : ControllerBase
{
    [HttpPost]
    public async Task<Comment> AddAsync(CommentEditModel model)
    {
        await commentEditModelValidator.ValidateAndThrowAsync(model);
        return await commentRepository.AddAsync(model.ToComment());
    }

    [HttpDelete("{commentId:int}")]
    public async Task<bool> DeleteByKeyAsync(int commentId)
        => await commentRepository.DeleteByKeyAsync(commentId);
}