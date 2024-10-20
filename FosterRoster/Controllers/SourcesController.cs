using FosterRoster.Domain;
using Microsoft.AspNetCore.Mvc;

namespace FosterRoster.Controllers;

[ApiController]
[Route("api/sources")]
public sealed class SourcesController(
    ISourceRepository sourceRepository
) : ControllerBase
{
    [HttpGet]
    public async Task<List<Source>> GetAllAsync()
        => await sourceRepository.GetAllAsync();

    [HttpGet("names")]
    public async Task<List<ListItem<int>>> GetAllNamesAsync()
        => await sourceRepository.GetAllNamesAsync();

}