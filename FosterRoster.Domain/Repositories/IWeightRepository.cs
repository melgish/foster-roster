namespace FosterRoster.Domain.Repositories;

public interface IWeightRepository
{
    /// <summary>
    /// Adds a new weight to the database for a given cat.
    /// </summary>
    /// <param name="weight">weight information about cat</param>
    /// <returns>Weight from database</returns>
    public Task<Weight> AddAsync(Weight weight);

    /// <summary>
    /// Delete the given weight from the database.
    /// </summary>
    /// <param name="felineId"></param>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public Task<bool> DeleteByKeyAsync(int felineId, DateTimeOffset dateTime);

    /// <summary>
    /// Get all weights for all cats in the database.
    /// </summary>
    /// <returns>Array of weights or null if there are none.</returns>
    public Task<List<Weight>> GetAllAsync();
}