namespace FosterRoster.Controllers;

[ApiController]
[Route("api/weights")]
public sealed class WeightsController(
    IValidator<WeightEditModel> weightEditModelValidator,
    IWeightRepository weightRepository
) : ControllerBase
{
    [HttpPost]
    public async Task<Weight> AddAsync(WeightEditModel model)
    {
        await weightEditModelValidator.ValidateAndThrowAsync(model);
        return await weightRepository.AddAsync(model.ToWeight());
    }

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
