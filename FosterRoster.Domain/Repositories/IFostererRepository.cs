namespace FosterRoster.Domain.Repositories;

public interface IFostererRepository
{
    /// <summary>
    /// Adds a new fosterer to the database.
    /// </summary>
    /// <param name="fosterer">Fosterer instance to add.</param>
    /// <returns>Updated feline instance after add.</returns>
    public Task<Fosterer> AddAsync(Fosterer fosterer);
    
    /// <summary>
    /// Deletes a Fosterer by its ID.
    /// </summary>
    /// <param name="fostererId">ID of fosterer to remove.</param>
    /// <returns>True if a fosterer was removed otherwise false.</returns>
    public Task<bool> DeleteByKeyAsync(int fostererId);
    public Task<List<Fosterer>> GetAllAsync();
    public Task<List<ListItem<int>>> GetAllNamesAsync();
    public Task<Fosterer?> GetByKeyAsync(int fostererId);
    
    /// <summary>
    /// Updates a Fosterer in the database.
    /// </summary>
    /// <param name="fostererId">ID of Fosterer to update</param>
    /// <param name="fosterer">Data to assign to Fosterer</param>
    /// <returns>Updated Fosterer if found, otherwise null</returns>
    public Task<Fosterer?> UpdateAsync(int fostererId, Fosterer fosterer);
}
