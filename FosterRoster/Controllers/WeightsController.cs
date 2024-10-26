namespace FosterRoster.Controllers;

[ApiController]
[Route("api/weights")]
public sealed class WeightsController(
    IValidator<WeightEditModel> weightEditModelValidator,
    IWeightRepository weightRepository
) : ControllerBase
{
    /// <summary>
    /// Adds a new weight entry for a Feline.
    /// </summary>
    /// <param name="model">Data about weight to add.</param>
    /// <returns>Updated weight after add.</returns>
    [HttpPost]
    public async Task<Weight> AddAsync(WeightEditModel model)
    {
        await weightEditModelValidator.ValidateAndThrowAsync(model);
        return await weightRepository.AddAsync(model.ToWeight());
    }

    /// <summary>
    /// Deletes a weight entry by its ID and time stamp
    /// </summary>
    /// <param name="felineId">ID of feline weight to delete.</param>
    /// <param name="dateTime">Date time of weight instance to delete.</param>
    /// <returns>True if a weight was deleted, otherwise false.</returns>
    [HttpDelete("{felineId:int}/{dateTime}")]
    public async Task<bool> DeleteByKeyAsync(int felineId, DateTimeOffset dateTime)
        => await weightRepository.DeleteByKeyAsync(felineId, dateTime);

    /// <summary>
    /// Get list of all weights in the database.
    /// </summary>
    /// <returns>List of weights, or empty list if no weights exist.</returns>
    [HttpGet]
    public async Task<List<Weight>> GetAllAsync()
        => await weightRepository.GetAllAsync();
}
