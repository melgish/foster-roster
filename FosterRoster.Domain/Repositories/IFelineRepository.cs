namespace FosterRoster.Domain.Repositories;

public interface IFelineRepository : IRepository
{
    /// <summary>
    ///     Restores identified feline to active status.
    /// </summary>
    /// <param name="felineId">ID of feline to update.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public Task<Result> ActivateAsync(int felineId);

    /// <summary>
    ///     Adds a new feline to the database.
    /// </summary>
    /// <param name="feline">Feline instance to add.</param>
    /// <returns>A Result with added feline, or errors on failure.</returns>
    public Task<Result<FelineEditModel>> AddAsync(FelineEditModel feline);

    /// <summary>
    ///     Sets a feline as inactive in the database.
    /// </summary>
    /// <param name="felineId">ID of feline to deactivate.</param>
    /// <param name="dateTimeUtc">Date and Time of deactivation.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public Task<Result> DeactivateAsync(int felineId, DateTimeOffset dateTimeUtc);

    /// <summary>
    ///     Deletes a feline by its ID.
    /// </summary>
    /// <param name="felineId">ID of feline to remove.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public Task<Result> DeleteByKeyAsync(int felineId);

    /// <summary>
    ///     Get list of all felines in the database.
    /// </summary>
    /// <returns>A Result with list of felines, or errors on failure.</returns>
    public Task<Result<List<Feline>>> GetAllAsync();

    /// <summary>
    ///     Get list of all felines in the database, with only their names and ids.
    /// </summary>
    /// <returns>A Result with list of items, or errors on failure.</returns>
    public Task<Result<List<ListItem<int>>>> GetAllNamesAsync();

    /// <summary>
    ///     Gets a single feline by ID.
    /// </summary>
    /// <param name="felineId">ID of feline to get.</param>
    /// <returns>A Result with Feline if found, or errors on failure</returns>
    public Task<Result<Feline>> GetByKeyAsync(int felineId);

    /// <summary>
    ///     Gets the thumbnail for a single feline.
    /// </summary>
    /// <param name="felineId">ID of the feline</param>
    /// <returns>A Result with Thumbnail if found, or errors on failure.</returns>
    public Task<Result<Thumbnail>> GetThumbnailAsync(int felineId);

    /// <summary>
    ///     Sets the thumbnail for a feline.
    /// </summary>
    /// <param name="felineId">ID of feline to change</param>
    /// <param name="thumbnail">Thumbnail to assign to feline</param>
    /// <returns>A Result with Feline if updated, or errors on failure.</returns>
    public Task<Result<Feline>> SetThumbnailAsync(int felineId, Thumbnail thumbnail);

    /// <summary>
    ///     Updates a feline in the database.
    /// </summary>
    /// <param name="felineId">ID of feline to update</param>
    /// <param name="feline">Data to assign to feline</param>
    /// <returns>A Result with Feline if updated, or errors on failure.</returns>
    public Task<Result<Feline>> UpdateAsync(int felineId, Feline feline);

    /// <summary>
    ///     Query for data
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="orderBy"></param>
    /// <param name="skip"></param>
    /// <param name="top"></param>
    /// <returns></returns>
    public Task<Result<QueryResults<Feline>>> QueryAsync(string? filter = null, int? top = null, int? skip = null,
        string? orderBy = null);
}