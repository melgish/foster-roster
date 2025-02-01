namespace FosterRoster.Domain.Repositories;

public interface ISourceRepository
{
    /// <summary>
    ///     Adds a new source
    /// </summary>
    /// <param name="source">Source to add</param>
    /// <returns>A Result with Source if successful, or Errors on failure.</returns>
    public Task<Result<Source>> AddAsync(Source source);

    /// <summary>
    ///     Get list of all sources in the database.
    /// </summary>
    /// <returns>A Result with list of sources if successful, or Errors on failure.</returns>
    public Task<Result<List<Source>>> GetAllAsync();

    /// <summary>
    ///     Get list of all sources in the database, with only their names and ids.
    /// </summary>
    /// <returns>A Result with list of items if successful, or Errors on failure.</returns>
    public Task<Result<List<ListItem<int>>>> GetAllNamesAsync();
}