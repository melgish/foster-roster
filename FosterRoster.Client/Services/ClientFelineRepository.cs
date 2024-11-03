namespace FosterRoster.Client.Services;

public sealed class ClientFelineRepository(
    HttpClient httpClient
) : IFelineRepository
{
    const string Route = "api/felines";

    public async Task<bool> Activate(int felineId)
    {
        var rs = await httpClient.PutAsync($"{Route}/{felineId}/activate", null);
        return await rs.Content.ReadFromJsonAsync<bool>();
    }

    /// <summary>
    /// Adds a new cat to the database.
    /// </summary>
    /// <param name="feline">Feline instance to add.</param>
    /// <returns>Updated feline instance after add.</returns>
    public async Task<Feline> AddAsync(Feline feline)
    {
        var rs = await httpClient.PostAsJsonAsync(Route, new FelineEditModel(feline));
        return await rs.Content.ReadFromJsonAsync<Feline>() ?? feline;
    }

    /// <summary>
    /// Deletes a Feline by its ID.
    /// </summary>
    /// <param name="felineId">ID of feline to remove.</param>
    /// <returns>True if a cat was removed otherwise false.</returns>
    public async Task<bool> DeleteByKeyAsync(int felineId)
    {
        var rs = await httpClient.DeleteAsync($"{Route}/{felineId}");
        return await rs.Content.ReadFromJsonAsync<bool>();
    }

    /// <summary>
    /// Get list of all cats in the database.
    /// </summary>
    /// <returns>List of cats, or empty list if no cats exist.</returns>
    public async Task<List<Feline>> GetAllAsync()
        => await httpClient.GetFromJsonAsync<List<Feline>>(Route) ?? [];

    /// <summary>
    /// Get list of all cats in the database.
    /// </summary>
    /// <returns>List of cats, or empty list if no cats exist.</returns>
    public async Task<List<ListItem<int>>> GetAllNamesAsync()
        => await httpClient.GetFromJsonAsync<List<ListItem<int>>>($"{Route}/names") ?? [];

    /// <summary>
    /// Gets a single cat by ID.
    /// </summary>
    /// <param name="felineId"></param>
    /// <returns>A single cat if found, otherwise null</returns>
    public async Task<Feline?> GetByKeyAsync(int felineId)
        => await httpClient.GetFromJsonAsync<Feline>($"{Route}/{felineId}");

    /// <summary>
    /// Gets the thumbnail for a single cat.
    /// </summary>
    /// <param name="felineId">ID of the cat</param>
    /// <returns>Thumbnail if found, otherwise null</returns>
    public async Task<Thumbnail?> GetThumbnailAsync(int felineId)
       => await httpClient.GetFromJsonAsync<Thumbnail>($"{Route}/{felineId}/thumbnail");

    /// <summary>
    /// Sets a cat as inactive in the database.
    /// </summary>
    /// <param name="felineId">id of cat to update</param>
    /// <param name="dateTimeUtc">date time to assign to inactivation</param>
    /// <returns>true if cat was modified, otherwise false</returns>
    public async Task<bool> Inactivate(int felineId, DateTimeOffset dateTimeUtc)
    {
        var model = new DateTimeEditModel(dateTimeUtc.DateTime);
        var rs = await httpClient.PutAsJsonAsync($"{Route}/{felineId}/inactivate", model);
        return await rs.Content.ReadFromJsonAsync<bool>();
    }

    /// <summary>
    /// Sets the thumbnail for a cat.
    /// </summary>
    /// <param name="felineId">ID of cat to change</param>
    /// <param name="thumbnail">Thumbnail to assign to cat</param>
    /// <returns>Updated cat, or null if cat was not found</returns>
    public async Task<Feline?> SetThumbnailAsync(int felineId, Thumbnail thumbnail)
    {
        var rs = await httpClient.PostAsJsonAsync($"{Route}/{felineId}/thumbnail", thumbnail);
        return await rs.Content.ReadFromJsonAsync<Feline>();
    }

    /// <summary>
    /// Updates a cat in the database.
    /// </summary>
    /// <param name="felineId">ID of cat to update</param>
    /// <param name="feline">Data to assign to cat</param>
    /// <returns>Updated cat if found, otherwise null</returns>
    public async Task<Feline?> UpdateAsync(int felineId, Feline feline)
    {
        var model = new FelineEditModel(feline);
        var rs = await httpClient.PutAsJsonAsync($"{Route}/{felineId}", model);
        return await rs.Content.ReadFromJsonAsync<Feline>();
    }
}