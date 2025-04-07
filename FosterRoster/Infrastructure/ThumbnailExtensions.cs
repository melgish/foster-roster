namespace FosterRoster.Infrastructure;

using Microsoft.AspNetCore.Components.Forms;
using System.Net.Mime;
using Features.Thumbnails;

public static class ThumbnailExtensions
{
    private const string NoImage =
        "data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='120' height='120' viewPort='0 0 200 200'%3E%3Ctext x='50%25' y='50%25' font-size='1rem' dominant-baseline='middle' text-anchor='middle'%3ENO IMAGE%3C/text%3E%3C/svg%3E";

    /// <summary>
    ///     Converts a <see cref="IBrowserFile" /> to a <see cref="Thumbnail" />.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="felineId"></param>
    /// <returns></returns>
    public static async Task<Thumbnail?> ToThumbnailAsync(this IBrowserFile? file, int felineId = 0)
    {
        if (file is null) return null;
        // Request a resized image as a PNG
        var image = await file.RequestImageFileAsync(MediaTypeNames.Image.Png, 256, 256);

        // Copy the image data into a byte array
        using var memory = new MemoryStream();
        await using var stream = image.OpenReadStream();
        await stream.CopyToAsync(memory);

        return new()
        {
            FelineId = felineId,
            ImageData = memory.ToArray(),
            ContentType = image.ContentType
        };
    }

    public static string GetUrl(int felineId, uint? version)
        => version is null ? NoImage : $"thumbnails/{felineId}?v={version}";

    public static string GetUrl(this Thumbnail? thumbnail)
        => thumbnail switch
        {
            { ImageData.Length: 0 } => $"thumbnails/{thumbnail.FelineId}?v={thumbnail.Version}",
            { ImageData.Length: > 0 } =>
                $"data:{thumbnail.ContentType};base64,{Convert.ToBase64String(thumbnail.ImageData)}",
            _ => NoImage
        };
}