namespace FosterRoster.Domain.Repositories;

public interface IWeightRepository
{
    /// <summary>
    /// Adds a new weight to the database for a given feline.
    /// </summary>
    /// <param name="weight">weight information about feline.</param>
    /// <returns>Result with Weight on success, or Errors on failure.</returns>
    public Task<Result<Weight>> AddAsync(Weight weight);

    /// <summary>
    /// Delete the given weight from the database.
    /// </summary>
    /// <param name="felineId">ID of feline.</param>
    /// <param name="dateTime">Date and Time of weight to remove.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public Task<Result> DeleteByKeyAsync(int felineId, DateTimeOffset dateTime);

    /// <summary>
    /// Get last 2 weeks of weights for all felines in the database.
    /// </summary>
    /// <returns>A Result with list of Weights success, or Errors on failure.</returns>
    public Task<Result<List<Weight>>> GetAllAsync();
}