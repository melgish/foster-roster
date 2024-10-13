namespace FosterRoster.Controllers;

using FluentValidation;

using FosterRoster.Domain;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/comments")]
public sealed class CommentsController(
    IValidator<CommentEditModel> commentEditModelValidator,
    ICommentRepository commentRepository
): ControllerBase
{
    [HttpPost]
    public async Task<Comment> AddAsync(CommentEditModel model)
    {
        await commentEditModelValidator.ValidateAndThrowAsync(model);
        return await commentRepository.AddAsync(model.ToComment());
    }

}