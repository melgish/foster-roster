namespace FosterRoster.Controllers;

using FluentValidation;
using FosterRoster.Domain;

using Ganss.Xss;

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
        // Sanitize the incoming HTML
        model.Text = new HtmlSanitizer().Sanitize(model.Text);
        return await commentRepository.AddAsync(model.ToComment());
    }

}