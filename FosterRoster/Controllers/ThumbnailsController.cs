namespace FosterRoster.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OutputCaching;

[ApiController]
[Route("thumbnails")]
public sealed class ThumbnailsController(
    FosterRosterDbContext dbContext
) : ControllerBase
{
    /// <summary>
    /// Gets thumbnail image as file
    /// </summary>
    /// <param name="felineId">ID of feline thumbnail to fetch.</param>
    /// <returns>File if found, otherwise 404</returns>
    [AllowAnonymous]
    [HttpGet("{felineId:int}")]
    [OutputCache(Duration = 60*60*24, VaryByQueryKeys = ["v"])]
    [ResponseCache(Duration = 60*60*24*7, VaryByQueryKeys = ["v"])]
    public async Task<IActionResult> GetThumbnailAsync(int felineId)
    {
        var thumbnail = await dbContext
            .Thumbnails
            .AsNoTracking()
            .Where(t => t.FelineId == felineId)
            .Select(t => new { t.ImageData, t.ContentType })
            .FirstOrDefaultAsync();
        if (thumbnail is not null)
            return new FileContentResult(
                thumbnail.ImageData,
                thumbnail.ContentType
            );
        return NotFound();
    }
}