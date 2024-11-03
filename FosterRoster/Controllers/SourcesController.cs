namespace FosterRoster.Controllers;

[ApiController]
[Route("api/sources")]
public sealed class SourcesController(
    ISourceRepository sourceRepository
) : ControllerBase
{
    /// <summary>
    /// Get list of all sources in the database.
    /// </summary>
    /// <returns>List of sources, or empty list if none exist.</returns>
    [HttpGet]
    public async Task<List<Source>> GetAllAsync()
        => await sourceRepository.GetAllAsync();

    /// <summary>
    /// Get list of all source names in the database.
    /// </summary>
    /// <returns>List of names, or empty list if none exist.</returns>
    [HttpGet("names")]
    public async Task<List<ListItem<int>>> GetAllNamesAsync()
        => await sourceRepository.GetAllNamesAsync();
}