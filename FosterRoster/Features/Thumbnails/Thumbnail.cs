namespace FosterRoster.Features.Thumbnails;

public sealed class Thumbnail
{
    /// <summary>
    ///     The ID of the feline this thumbnail is associated with.
    /// </summary>
    public int FelineId { get; init; }

    /// <summary>
    ///     The image data for the thumbnail.
    /// </summary>
    public byte[] ImageData { get; set; } = [];

    /// <summary>
    ///     The version of the thumbnail, used for cache busting.
    /// </summary>
    public uint Version { get; init; }

    /// <summary>
    ///     The content type of the image data. Will be image/png unless something
    ///     terrible happens.
    /// </summary>
    public string ContentType { get; set; } = string.Empty;
}