namespace FosterRoster.Domain;

public interface IFelineRepository
{
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
    public Task<Feline?> GetByIdAsync(int felineId);

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