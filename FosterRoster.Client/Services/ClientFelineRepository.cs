namespace FosterRoster.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http.Json;
using FosterRoster.Domain;

public sealed class ClientFelineRepository(
    HttpClient httpClient
) : IFelineRepository
{
    const string route = "api/felines";
    /// <summary>
    /// Adds a new cat to the database.
    /// </summary>
    /// <param name="feline">Feline instance to add.</param>
    /// <returns>Updated feline instance after add.</returns>
    public async Task<Feline> AddAsync(Feline feline)
    {
        var rs = await httpClient.PostAsJsonAsync<FelineEditModel>(route, new(feline));
        return await rs.Content.ReadFromJsonAsync<Feline>() ?? feline;
    }

    /// <summary>
    /// Deletes a cat by it's id.
    /// </summary>
    /// <param name="felineId">Id of feline to remove.</param>
    /// <returns>True if a cat was removed otherwise false.</returns>
    public async Task<bool> DeleteByKeyAsync(int felineId)
    {
        var rs = await httpClient.DeleteAsync($"{route}/{felineId}");
        return await rs.Content.ReadFromJsonAsync<bool>();
    }

    /// <summary>
    /// Get list of all cats in the database.
    /// </summary>
    /// <returns>List of cats, or empty list if no cats exist.</returns>
    public async Task<List<Feline>> GetAllAsync()
        => await httpClient.GetFromJsonAsync<List<Feline>>(route) ?? [];

    /// <summary>
    /// Get list of all cats in the database.
    /// </summary>
    /// <returns>List of cats, or empty list if no cats exist.</returns>
    public async Task<List<ListItem<int>>> GetAllNamesAsync()
        => await httpClient.GetFromJsonAsync<List<ListItem<int>>>($"{route}/names") ?? [];

    /// <summary>
    /// Gets a single cat by ID.
    /// </summary>
    /// <param name="felineId"></param>
    /// <returns>A single cat if found, otherwise null</returns>
    public async Task<Feline?> GetByIdAsync(int felineId)
        => await httpClient.GetFromJsonAsync<Feline>($"{route}/{felineId}");

    /// <summary>
    /// Gets the thumbnail for a single cat.
    /// </summary>
    /// <param name="felineId">Id of the cat</param>
    /// <returns>Thumbnail if found, otherwise null</returns>
    public async Task<Thumbnail?> GetThumbnailAsync(int felineId)
       => await httpClient.GetFromJsonAsync<Thumbnail>($"{route}/{felineId}/thumbnail");

    /// <summary>
    /// Sets the thumbnail for a cat.
    /// </summary>
    /// <param name="felineId">Id of cat to change</param>
    /// <param name="thumbnail">Thumbnail to assign to cat</param>
    /// <returns>Updated cat, or null if cat was not found</returns>
    public async Task<Feline?> SetThumbnailAsync(int felineId, Thumbnail thumbnail)
    {
        var rs = await httpClient.PostAsJsonAsync($"{route}/{felineId}/thumbnail", thumbnail);
        return await rs.Content.ReadFromJsonAsync<Feline>();
    }

    /// <summary>
    /// Updates a cat in the database.
    /// </summary>
    /// <param name="felineId">Id of cat to update</param>
    /// <param name="feline">Data to assign to cat</param>
    /// <returns>Updated cat if found, otherwise null</returns>
    public async Task<Feline?> UpdateAsync(int felineId, Feline feline)
    {
        var rs = await httpClient.PutAsJsonAsync($"{route}/{felineId}", new FelineEditModel(feline));
        return await rs.Content.ReadFromJsonAsync<Feline>();
    }
}