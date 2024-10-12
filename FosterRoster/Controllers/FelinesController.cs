namespace FosterRoster.Controllers;

using FluentValidation;

using FosterRoster.Domain;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/felines")]
public sealed class FelinesController(
    IValidator<FelineEditModel> felineEditModelValidator,
    IValidator<DateTimeEditModel> dateTimeEditModelValidator,
    IFelineRepository felineRepository
) : ControllerBase
{
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
    public async Task<Feline?> GetByIdAsync(int felineId)
        => await felineRepository.GetByIdAsync(felineId);

    /// <summary>
    /// Gets the thumbnail for a single cat.
    /// </summary>
    /// <param name="felineId">Id of the cat</param>
    /// <returns>Thumbnail if found, otherwise null</returns>
    [HttpGet("{felineId:int}/thumbnail")]
    public async Task<Thumbnail?> GetThumbnailAsync(int felineId)
        => await felineRepository.GetThumbnailAsync(felineId);

    [HttpPut("{felineId:int}/inactivate")]
    public async Task<bool> InactivateAsync(int felineId, DateTimeEditModel model)
    {
        await dateTimeEditModelValidator.ValidateAndThrowAsync(model);
        return await felineRepository.Inactivate(felineId, model.Value!.Value);
    }

    /// <summary>
    /// Sets the thumbnail for a cat.
    /// </summary>
    /// <param name="felineId">Id of cat to change</param>
    /// <param name="thumbnail">Thumbnail to assign to cat</param>
    /// <returns>Updated cat, or null if cat was not found</returns>
    [HttpPost("{felineId:int}/thumbnail")]
    public async Task<Feline?> SetThumbnailAsync(int felineId, [FromBody] Thumbnail thumbnail)
        => await felineRepository.SetThumbnailAsync(felineId, thumbnail);

    /// <summary>
    /// Updates a cat in the database.
    /// </summary>
    /// <param name="felineId">Id of cat to update</param>
    /// <param name="feline">Data to assign to cat</param>
    /// <returns>Updated cat if found, otherwise null</returns>
    [HttpPut("{felineId:int}")]
    public async Task<Feline?> UpdateAsync(int felineId, FelineEditModel model)
    {
        await felineEditModelValidator.ValidateAndThrowAsync(model);
        return await felineRepository.UpdateAsync(felineId, model.ToFeline());
    }
}
