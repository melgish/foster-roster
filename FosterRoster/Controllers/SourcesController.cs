namespace FosterRoster.Controllers;

[ApiController]
[Route("api/sources")]
public sealed class SourcesController(
    ISourceRepository sourceRepository
) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Source>> AddAsync(Source source)
        => await sourceRepository.AddAsync(source) switch
        {
            { IsSuccess: true } ok => Created($"/api/sources/{ok.Value.Id}", ok.Value),
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
}