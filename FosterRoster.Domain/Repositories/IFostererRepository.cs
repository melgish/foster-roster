namespace FosterRoster.Domain.Repositories;

public interface IFostererRepository
{
    public Task<Fosterer> AddAsync(Fosterer fosterer);
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
