namespace FosterRoster.Controllers;

[ApiController]
[Route("api/weights")]
public sealed class WeightsController(
    IWeightRepository weightRepository
) : ControllerBase
{
    /// <summary>
    ///     Adds a new weight entry for a Feline.
    /// </summary>
    /// <param name="model">Data about weight to add.</param>
    /// <returns>Updated weight after add.</returns>
    [HttpPost]
    public async Task<ActionResult<Weight>> AddAsync(WeightEditModel model)
        => await weightRepository.AddAsync(model.ToWeight()) switch
        {
            { IsSuccess: true } ok => Ok(ok.Value),
            { } err => this.Unprocessable(err)
        };

    /// <summary>
    ///     Deletes a weight entry by its ID and time stamp
    /// </summary>
    /// <param name="felineId">ID of feline weight to delete.</param>
    /// <param name="dateTime">Date time of weight instance to delete.</param>
    /// <returns>True if a weight was deleted, otherwise false.</returns>
    [HttpDelete("{felineId:int}/{dateTime}")]
    public async Task<IActionResult> DeleteByKeyAsync(int felineId, DateTimeOffset dateTime)
        => await weightRepository.DeleteByKeyAsync(felineId, dateTime) switch
        {
            { IsSuccess: true } => NoContent(),
            { } err when err.HasError<NotFoundError>() => NotFound(),
            { } err => this.Unprocessable(err)
        };

    /// <summary>
    ///     Get list of all weights in the database.
    /// </summary>
    /// <returns>List of weights, or empty list if no weights exist.</returns>
    [HttpGet]
    public async Task<ActionResult<List<Weight>>> GetAllAsync()
        => await weightRepository.GetAllAsync() switch
        {
            { IsSuccess: true } ok => Ok(ok.Value),
            { } err => this.Unprocessable(err)
        };
}