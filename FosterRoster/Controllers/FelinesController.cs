namespace FosterRoster.Controllers;

[ApiController]
[Route("api/felines")]
public sealed class FelinesController(
    IFelineRepository felineRepository
) : ControllerBase
{
    /// <summary>
    ///     Reactivates a previously inactivated Feline.
    /// </summary>
    /// <param name="felineId">Unique identifier for feline to activate.</param>
    /// <returns>True if a feline was activated, otherwise false.</returns>
    [HttpPut("{felineId:int}/activate")]
    public async Task<IActionResult> ActivateAsync(int felineId)
        => await felineRepository.ActivateAsync(felineId) switch
        {
            { IsSuccess: true } => NoContent(),
            { } err => this.Unprocessable(err)
        };

    /// <summary>
    ///     Adds a new feline to the database.
    /// </summary>
    /// <param name="model">Model containing feline data to add.</param>
    /// <returns>Updated feline instance after add.</returns>
    [HttpPost]
    public async Task<ActionResult<Feline>> AddAsync(FelineEditModel model)
        => await felineRepository.AddAsync(model) switch
        {
            { IsSuccess: true } ok => Created($"/api/felines/{ok.Value.Id}", ok.Value),
            { } err => this.Unprocessable(err)
        };

    /// <summary>
    ///     Deactivates the indicated feline using supplied AsOf date
    /// </summary>
    /// <param name="felineId">ID of the Feline to modify.</param>
    /// <param name="model">Inactivation date and time.</param>
    /// <returns>True if a feline was inactivated, otherwise false.</returns>
    [HttpPut("{felineId:int}/inactivate")]
    [HttpPut("{felineId:int}/deactivate")]
    public async Task<IActionResult> DeactivateAsync(int felineId, DateTimeEditModel model)
        => await felineRepository.DeactivateAsync(felineId, model.Value!.Value) switch
        {
            { IsSuccess: true } => NoContent(),
            { } err => this.Unprocessable(err)
        };

    /// <summary>
    ///     Deletes a cat by its ID.
    /// </summary>
    /// <param name="felineId">ID of feline to remove.</param>
    /// <returns>True if a feline was removed otherwise false.</returns>
    [HttpDelete("{felineId:int}")]
    public async Task<IActionResult> DeleteByKeyAsync(int felineId)
        => await felineRepository.DeleteByKeyAsync(felineId) switch
        {
            { IsSuccess: true } => NoContent(),
            { } err when err.HasError<NotFoundError>() => NotFound(),
            { } err => this.Unprocessable(err)
        };

    /// <summary>
    ///     Get list of all felines in the database.
    /// </summary>
    /// <returns>List of felines, or empty list if no felines exist.</returns>
    [HttpGet]
    public async Task<ActionResult<List<Feline>>> GetAllAsync()
        => await felineRepository.GetAllAsync() switch
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
        => await felineRepository.GetAllNamesAsync() switch
        {
            { IsSuccess: true } ok => Ok(ok.Value),
            { } err => this.Unprocessable(err)
        };

    /// <summary>
    ///     Gets a single cat by ID.
    /// </summary>
    /// <param name="felineId"></param>
    /// <returns>A single cat if found, otherwise null</returns>
    [HttpGet("{felineId:int}")]
    public async Task<ActionResult<Feline>> GetByKeyAsync(int felineId)
        => await felineRepository.GetByKeyAsync(felineId) switch
        {
            { IsSuccess: true } ok => Ok(ok.Value),
            { } err when err.HasError<NotFoundError>() => NotFound(),
            { } err => this.Unprocessable(err)
        };

    /// <summary>
    ///     Gets the thumbnail for a single Feline.
    /// </summary>
    /// <param name="felineId">ID of the Feline.</param>
    /// <returns>Thumbnail instance if found, otherwise null</returns>
    [HttpGet("{felineId:int}/thumbnail")]
    public async Task<ActionResult<Thumbnail>> GetThumbnailAsync(int felineId)
        => await felineRepository.GetThumbnailAsync(felineId) switch
        {
            { IsSuccess: true } ok => Ok(ok.Value),
            { } err when err.HasError<NotFoundError>() => NotFound(),
            { } err => this.Unprocessable(err)
        };

    /// <summary>
    ///     Sets the thumbnail image for a Feline.
    /// </summary>
    /// <param name="felineId">ID of cat to change</param>
    /// <param name="thumbnail">Thumbnail to assign to cat</param>
    /// <returns>Updated cat, or null if cat was not found</returns>
    [HttpPost("{felineId:int}/thumbnail")]
    public async Task<ActionResult<Feline>> SetThumbnailAsync(int felineId, [FromBody] Thumbnail thumbnail)
        => await felineRepository.SetThumbnailAsync(felineId, thumbnail) switch
        {
            { IsSuccess: true } ok => Ok(ok.Value),
            { } err when err.HasError<NotFoundError>() => NotFound(),
            { } err => this.Unprocessable(err)
        };

    /// <summary>
    ///     Updates a Feline in the database.
    /// </summary>
    /// <param name="felineId">ID of Feline to modify</param>
    /// <param name="model">Updated data to assign</param>
    /// <returns>Updated Feline if found, otherwise null</returns>
    [HttpPut("{felineId:int}")]
    public async Task<ActionResult<Feline>> UpdateAsync(int felineId, FelineEditModel model)
        => await felineRepository.UpdateAsync(felineId, model.ToFeline()) switch
        {
            { IsSuccess: true } ok => Ok(ok.Value),
            { } err when err.HasError<NotFoundError>() => NotFound(),
            { } err => this.Unprocessable(err)
        };
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="skip"></param>
    /// <param name="top"></param>
    /// <param name="orderBy"></param>
    /// <returns></returns>
    [HttpGet("query")]
    public async Task<ActionResult<QueryResults<Feline>>> QueryAsync(
        string? filter = null,
        int? top = null,
        int? skip = null,
        string? orderBy = null)
        => await felineRepository.QueryAsync(filter, top, skip, orderBy) switch
        {
            { IsSuccess: true } ok => Ok(ok.Value),
            { } err => this.Unprocessable(err)
        };
}