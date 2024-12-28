namespace FosterRoster.Controllers;

[ApiController]
[Route("api/fosterers")]
public sealed class FosterersController(
    IFostererRepository fostererRepository
) : ControllerBase
{
    /// <summary>
    ///     Adds a new fosterer to the database.
    /// </summary>
    /// <param name="model">Model containing fosterer data to add.</param>
    /// <returns>Updated fosterer instance after add.</returns>
    [HttpPost]
    public async Task<ActionResult<Fosterer>> AddAsync(FostererEditModel model)
        => await fostererRepository.AddAsync(model.ToFosterer()) switch
        {
            { IsSuccess: true } ok => Created($"api/fosterers{ok.Value.Id}", ok.Value),
            { } err => this.Unprocessable(err)
        };

    /// <summary>
    ///     Deletes a fosterer by its ID.
    /// </summary>
    /// <param name="fostererId">ID of fosterer to remove.</param>
    /// <returns>True if a fosterer was removed otherwise false.</returns>
    [HttpDelete("{fostererId:int}")]
    public async Task<IActionResult> DeleteByKeyAsync(int fostererId)
        => await fostererRepository.DeleteByKeyAsync(fostererId) switch
        {
            { IsSuccess: true } => NoContent(),
            { } err when err.HasError<NotFoundError>() => NotFound(),
            { } err => this.Unprocessable(err)
        };

    /// <summary>
    ///     Get list of all cats in the database.
    /// </summary>
    /// <returns>List of cats, or empty list if no cats exist.</returns>
    [HttpGet]
    public async Task<ActionResult<List<Fosterer>>> GetAllAsync()
        => await fostererRepository.GetAllAsync() switch
        {
            { IsSuccess: true } ok => Ok(ok.Value),
            { } err => this.Unprocessable(err)
        };

    /// <summary>
    ///     Get list of all cats in the database.
    /// </summary>
    /// <returns>List of cats, or empty list if no cats exist.</returns>
    [HttpGet("names")]
    public async Task<ActionResult<List<ListItem<int>>>> GetAllNamesAsync()
        => await fostererRepository.GetAllNamesAsync() switch
        {
            { IsSuccess: true } ok => Ok(ok.Value),
            { } err => this.Unprocessable(err)
        };

    /// <summary>
    ///     Gets a single cat by ID.
    /// </summary>
    /// <param name="fostererId"></param>
    /// <returns>A single cat if found, otherwise null</returns>
    [HttpGet("{fostererId:int}")]
    public async Task<ActionResult<Fosterer>> GetByKeyAsync(int fostererId)
        => await fostererRepository.GetByKeyAsync(fostererId) switch
        {
            { IsSuccess: true } ok => Ok(ok.Value),
            { } err when err.HasError<NotFoundError>() => NotFound(),
            { } err => this.Unprocessable(err)
        };

    /// <summary>
    ///     Updates a Fosterer in the database.
    /// </summary>
    /// <param name="fostererId">ID of Fosterer to modify</param>
    /// <param name="model">Updated data to assign</param>
    /// <returns>Updated Fosterer if found, otherwise null</returns>
    [HttpPut("{fostererId:int}")]
    public async Task<ActionResult<Fosterer>> UpdateAsync(int fostererId, FostererEditModel model)
        => await fostererRepository.UpdateAsync(fostererId, model.ToFosterer()) switch
        {
            { IsSuccess: true } ok => Ok(ok.Value),
            { } err when err.HasError<NotFoundError>() => NotFound(),
            { } err => this.Unprocessable(err)
        };
}