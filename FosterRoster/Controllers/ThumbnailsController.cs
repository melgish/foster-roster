namespace FosterRoster.Controllers;

using FosterRoster.Domain;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/thumbnails")]
public sealed class ThumbnailsController(
    IFelineRepository felineRepository
): ControllerBase
{
    [HttpGet("{felineId}")]
    public async Task<IActionResult> GetThumbnailAsync(int felineId)
    {
        var thumbnail = await felineRepository.GetThumbnailAsync(felineId);
        if (thumbnail is not null)
        {
            return new FileContentResult(
                thumbnail.ImageData,
                thumbnail.ContentType
            );
        }
        return NotFound();
    }
}
