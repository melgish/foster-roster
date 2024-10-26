namespace FosterRoster.Controllers;

[ApiController]
[Route("api/felines")]
public sealed class FelinesController(
    IFelineRepository felineRepository,
    IValidator<FelineEditModel> felineEditModelValidator,
    IValidator<DateTimeEditModel> dateTimeEditModelValidator
) : ControllerBase
{
    /// <summary>
    /// Reactivates a previously inactivated Feline.
    /// </summary>
    /// <param name="felineId">Unqiue identifier for feline to activate.</param>
    /// <returns>True if a feline was activated, otherwise false.</returns>
    [HttpPut("{felineId:int}/activate")]
    public async Task<bool> ActivateAsync(int felineId)
        => await felineRepository.Activate(felineId);

    /// <summary>
    /// Adds a new cat to the database.
    /// </summary>
    /// <param name="model">Model containing feline data to add.</param>
    /// <returns>Updated feline instance after add.</returns>
    [HttpPost]
    public async Task<Feline> AddAsync(FelineEditModel model)
    {
        await felineEditModelValidator.ValidateAndThrowAsync(model);
        return await felineRepository.AddAsync(model.ToFeline());
    }

    /// <summary>
    /// Deletes a cat by it's id.
    /// </summary>
    /// <param name="felineId">Id of feline to remove.</param>
    /// <returns>True if a cat was removed otherwise false.</returns>
    [HttpDelete("{felineId:int}")]
    public async Task<bool> DeleteByKeyAsync(int felineId)
        => await felineRepository.DeleteByKeyAsync(felineId);

    /// <summary>
    /// Get list of all cats in the database.
    /// </summary>
    /// <returns>List of cats, or empty list if no cats exist.</returns>
    [HttpGet]
    public async Task<List<Feline>> GetAllAsync()
        => await felineRepository.GetAllAsync();

    /// <summary>
    /// Get list of all cats in the database.
    /// </summary>
    /// <returns>List of cats, or empty list if no cats exist.</returns>
    [HttpGet("names")]
    public async Task<List<ListItem<int>>> GetAllNamesAsync()
        => await felineRepository.GetAllNamesAsync();

    /// <summary>
    /// Gets a single cat by ID.
    /// </summary>
    /// <param name="felineId"></param>
    /// <returns>A single cat if found, otherwise null</returns>
    [HttpGet("{felineId:int}")]
    public async Task<Feline?> GetByKeyAsync(int felineId)
        => await felineRepository.GetByKeyAsync(felineId);

    /// <summary>
    /// Gets the thumbnail for a single Feline.
    /// </summary>
    /// <param name="felineId">ID of the Feline.</param>
    /// <returns>Thumbnail instance if found, otherwise null</returns>
    [HttpGet("{felineId:int}/thumbnail")]
    public async Task<Thumbnail?> GetThumbnailAsync(int felineId)
        => await felineRepository.GetThumbnailAsync(felineId);

    /// <summary>
    /// Deactivates the indicated feline using supplied AsOf date 
    /// </summary>
    /// <param name="felineId">ID of the Feline to modifiy.</param>
    /// <param name="model">Inactivation date and time.</param>
    /// <returns>True if a feline was inactivated, otherwise false.</returns>
    [HttpPut("{felineId:int}/inactivate")]
    public async Task<bool> InactivateAsync(int felineId, DateTimeEditModel model)
    {
        await dateTimeEditModelValidator.ValidateAndThrowAsync(model);
        return await felineRepository.Inactivate(felineId, model.Value!.Value);
    }

    /// <summary>
    /// Sets the thumbnail image for a Feline.
    /// </summary>
    /// <param name="felineId">Id of cat to change</param>
    /// <param name="thumbnail">Thumbnail to assign to cat</param>
    /// <returns>Updated cat, or null if cat was not found</returns>
    [HttpPost("{felineId:int}/thumbnail")]
    public async Task<Feline?> SetThumbnailAsync(int felineId, [FromBody] Thumbnail thumbnail)
        => await felineRepository.SetThumbnailAsync(felineId, thumbnail);

    /// <summary>
    /// Updates a Feline in the database.
    /// </summary>
    /// <param name="felineId">ID of Feline to modify</param>
    /// <param name="model">Updated data to assign</param>
    /// <returns>Updated Feline if found, otherwise null</returns>
    [HttpPut("{felineId:int}")]
    public async Task<Feline?> UpdateAsync(int felineId, FelineEditModel model)
    {
        await felineEditModelValidator.ValidateAndThrowAsync(model);
        return await felineRepository.UpdateAsync(felineId, model.ToFeline());
    }
}
