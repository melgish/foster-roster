namespace FosterRoster.Controllers;

[ApiController]
[Route("api/thumbnails")]
public sealed class ThumbnailsController(
    IFelineRepository felineRepository
) : ControllerBase
{
    /// <summary>
    /// Gets thumbnail image as file
    /// </summary>
    /// <param name="felineId">ID of feline thumbnail to fetch.</param>
    /// <returns>File if found, otherwise 404</returns>
    [HttpGet("{felineId:int}")]
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
