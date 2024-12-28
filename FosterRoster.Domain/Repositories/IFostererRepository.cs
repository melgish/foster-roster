namespace FosterRoster.Domain.Repositories;

public interface IFostererRepository
{
    /// <summary>
    ///     Adds a new fosterer to the database.
    /// </summary>
    /// <param name="fosterer">Fosterer instance to add.</param>
    /// <returns>Result with Fosterer after add, or Errors on failure.</returns>
    public Task<Result<Fosterer>> AddAsync(Fosterer fosterer);

    /// <summary>
    ///     Deletes a Fosterer by its ID.
    /// </summary>
    /// <param name="fostererId">ID of fosterer to remove.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public Task<Result> DeleteByKeyAsync(int fostererId);

    /// <summary>
    ///     Gets list of all fosterers from the database.
    /// </summary>
    /// <returns>Result with list of fosterers, or Errors on failure.</returns>
    public Task<Result<List<Fosterer>>> GetAllAsync();

    /// <summary>
    ///     Gets names of all fosterers from the database.
    /// </summary>
    /// <returns>Result with list of items, or Errors on failure.</returns>
    public Task<Result<List<ListItem<int>>>> GetAllNamesAsync();

    /// <summary>
    ///     Gets single fosterer from the database.
    /// </summary>
    /// <param name="fostererId">ID of fosterer to return.</param>
    /// <returns>Result with Fosterer if successful, or Errors on failure.</returns>
    public Task<Result<Fosterer>> GetByKeyAsync(int fostererId);

    /// <summary>
    ///     Updates an existing Fosterer in the database.
    /// </summary>
    /// <param name="fostererId">ID of Fosterer to update</param>
    /// <param name="fosterer">Data to assign to Fosterer</param>
    /// <returns>Result with updated Fosterer if found, or Errors on failure.</returns>
    public Task<Result<Fosterer>> UpdateAsync(int fostererId, Fosterer fosterer);
}