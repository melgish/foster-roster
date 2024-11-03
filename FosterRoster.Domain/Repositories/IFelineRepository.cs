namespace FosterRoster.Domain.Repositories;

public interface IFelineRepository
{
    /// <summary>
    /// Restores identified cat to active status.
    /// </summary>
    /// <param name="felineId">Id of cat to update</param>
    /// <returns>true if a cat was updated, otherwise false</returns>
    public Task<bool> Activate(int felineId);

    /// <summary>
    /// Adds a new cat to the database.
    /// </summary>
    /// <param name="feline">Feline instance to add.</param>
    /// <returns>Updated feline instance after add.</returns>
    public Task<Feline> AddAsync(Feline feline);

    /// <summary>
    /// Deletes a cat by it's id.
    /// </summary>
    /// <param name="felineId">Id of cat to remove.</param>
    /// <returns>True if a cat was removed otherwise false.</returns>
    public Task<bool> DeleteByKeyAsync(int felineId);

    /// <summary>
    /// Get list of all cats in the database.
    /// </summary>
    /// <returns>List of cats, or empty list if no cats exist.</returns>
    public Task<List<Feline>> GetAllAsync();

    /// <summary>
    /// Get list of all cats in the database, with only their names and ids.
    /// </summary>
    /// <returns></returns>
    public Task<List<ListItem<int>>> GetAllNamesAsync();

    /// <summary>
    /// Gets a single cat by ID.
    /// </summary>
    /// <param name="felineId">Id of cat to get.</param>
    /// <returns>A single cat if found, otherwise null</returns>
    public Task<Feline?> GetByKeyAsync(int felineId);

    /// <summary>
    /// Sets a cat as inactive in the database.
    /// </summary>
    /// <param name="felineId"></param>
    /// <param name="dateTimeUtc"></param>
    /// <returns></returns>
    public Task<bool> Inactivate(int felineId, DateTimeOffset dateTimeUtc);

    /// <summary>
    /// Gets the thumbnail for a single cat.
    /// </summary>
    /// <param name="felineId">Id of the cat</param>
    /// <returns>Thumbnail if found, otherwise null</returns>
    public Task<Thumbnail?> GetThumbnailAsync(int felineId);

    /// <summary>
    /// Sets the thumbnail for a cat.
    /// </summary>
    /// <param name="felineId">Id of cat to change</param>
    /// <param name="thumbnail">Thumbnail to assign to cat</param>
    /// <returns>Updated cat, or null if cat was not found</returns>
    public Task<Feline?> SetThumbnailAsync(int felineId, Thumbnail thumbnail);

    /// <summary>
    /// Updates a cat in the database.
    /// </summary>
    /// <param name="felineId">Id of cat to update</param>
    /// <param name="feline">Data to assign to cat</param>
    /// <returns>Updated cat if found, otherwise null</returns>
    public Task<Feline?> UpdateAsync(int felineId, Feline feline);
}