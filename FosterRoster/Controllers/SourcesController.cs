namespace FosterRoster.Controllers;

[ApiController]
[Route("api/sources")]
public sealed class SourcesController(
    ISourceRepository sourceRepository
) : ControllerBase
{
    /// <summary>
    ///     Adds a new source
    /// </summary>
    /// <param name="source">Source to add</param>
    /// <returns>A Result with Source if successful, or Errors on failure.</returns>
    [HttpPost]
    public async Task<ActionResult<Source>> AddAsync(Source source)
        => await sourceRepository.AddAsync(source) switch
        {
            { IsSuccess: true } ok => Created($"/api/sources/{ok.Value.Id}", ok.Value),
            { } err => this.Unprocessable(err)
        };

    /// <summary>
    ///     Deletes a Source by its ID.
    /// </summary>
    /// <param name="sourceId">ID of Source to remove.</param>
    /// <returns>True if a Source was removed otherwise false.</returns>
    [HttpDelete("{sourceId:int}")]
    public async Task<IActionResult> DeleteByKeyAsync(int sourceId)
        => await sourceRepository.DeleteByKeyAsync(sourceId) switch
        {
            { IsSuccess: true } => NoContent(),
            { } err when err.HasError<NotFoundError>() => NotFound(),
            { } err => this.Unprocessable(err)
        };

    /// <summary>
    ///     Get list of all sources in the database.
    /// </summary>
    /// <returns>List of sources, or empty list if none exist.</returns>
    [HttpGet]
    public async Task<ActionResult<List<Source>>> GetAllAsync()
        => await sourceRepository.GetAllAsync() switch
        {
            { IsSuccess: true } ok => Ok(ok.Value),
            { } err => this.Unprocessable(err)
        };

    /// <summary>
    ///     Get list of all source names in the database.
    /// </summary>
    /// <returns>List of names, or empty list if none exist.</returns>
    [HttpGet("names")]
    public async Task<ActionResult<List<ListItem<int>>>> GetAllNamesAsync()
        => await sourceRepository.GetAllNamesAsync() switch
        {
            { IsSuccess: true } ok => Ok(ok.Value),
            { } err => this.Unprocessable(err)
        };

    /// <summary>
    ///     Gets a single Source by ID.
    /// </summary>
    /// <param name="sourceId">ID of the Source to retrieve.</param>
    /// <returns>A single Source if found, otherwise null</returns>
    [HttpGet("{sourceId:int}")]
    public async Task<ActionResult<Fosterer>> GetByKeyAsync(int sourceId)
        => await sourceRepository.GetByKeyAsync(sourceId) switch
        {
            { IsSuccess: true } ok => Ok(ok.Value),
            { } err when err.HasError<NotFoundError>() => NotFound(),
            { } err => this.Unprocessable(err)
        };

    /// <summary>
    ///     Updates a Source in the database.
    /// </summary>
    /// <param name="sourceId">ID of Source to modify</param>
    /// <param name="model">Updated data to assign</param>
    /// <returns>Updated Source if found, otherwise null</returns>
    [HttpPut("{sourceId:int}")]
    public async Task<ActionResult<Source>> UpdateAsync(int sourceId, SourceEditModel model)
        => await sourceRepository.UpdateAsync(sourceId, model.ToSource()) switch
        {
            { IsSuccess: true } ok => Ok(ok.Value),
            { } err when err.HasError<NotFoundError>() => NotFound(),
            { } err => this.Unprocessable(err)
        };
}