namespace FosterRoster.Domain.Repositories;

public interface ISourceRepository : IRepository
{
    /// <summary>
    ///     Adds a new source
    /// </summary>
    /// <param name="source">Source to add</param>
    /// <returns>A Result with Source if successful, or Errors on failure.</returns>
    public Task<Result<Source>> AddAsync(Source source);

    /// <summary>
    ///     Deletes a Source by its ID.
    /// </summary>
    /// <param name="sourceId">ID of source to remove.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public Task<Result> DeleteByKeyAsync(int sourceId);

    /// <summary>
    ///     Get list of all Sources in the database.
    /// </summary>
    /// <returns>A Result with list of sources if successful, or Errors on failure.</returns>
    public Task<Result<List<Source>>> GetAllAsync();

    /// <summary>
    ///     Gets single Source from the database.
    /// </summary>
    /// <param name="sourceId">ID of source to return.</param>
    /// <returns>Result with Source if successful, or Errors on failure.</returns>
    public Task<Result<Source>> GetByKeyAsync(int sourceId);

    /// <summary>
    ///     Updates an existing Source in the database.
    /// </summary>
    /// <param name="sourceId">ID of Source to update</param>
    /// <param name="source">Data to assign to Source</param>
    /// <returns>Result with updated Source if found, or Errors on failure.</returns>
    public Task<Result<Source>> UpdateAsync(int sourceId, Source source);
}