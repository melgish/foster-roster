namespace FosterRoster.Features.Felines;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

[ApiController]
[Route("thumbnails")]
public sealed class ThumbnailsController(
    FelineRepository felineRepository
) : ControllerBase
{
    /// <summary>
    ///     Gets thumbnail image as file
    /// </summary>
    /// <param name="felineId">ID of feline thumbnail to fetch.</param>
    /// <returns>File if found, otherwise 404</returns>
    [AllowAnonymous]
    [HttpGet("{felineId:int}")]
    [OutputCache(Duration = 60 * 60 * 24, VaryByQueryKeys = ["v"])]
    [ResponseCache(Duration = 60 * 60 * 24 * 7, VaryByQueryKeys = ["v"])]
    public async Task<IActionResult> GetThumbnailAsync(int felineId)
    {
        var rs = await felineRepository.GetThumbnailAsync(felineId);
        return rs.IsSuccess
            ? new FileContentResult(rs.Value.ImageData, rs.Value.ContentType)
            : NotFound();
    }
}