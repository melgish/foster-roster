namespace FosterRoster.Client.Services;
public sealed class ClientFostererRepository(
    HttpClient httpClient
) : IFostererRepository
{
    private const string Route = "api/fosterers";

    /// <summary>
    /// Adds a new fosterer to the database.
    /// </summary>
    /// <param name="fosterer">Fosterer instance to add.</param>
    /// <returns>Updated feline instance after add.</returns>
    public async Task<Fosterer> AddAsync(Fosterer fosterer)
    {
        var rs = await httpClient.PostAsJsonAsync(Route, new FostererEditModel(fosterer));
        return await rs.Content.ReadFromJsonAsync<Fosterer>() ?? fosterer;
    }

    /// <summary>
    /// Deletes a Fosterer by its ID.
    /// </summary>
    /// <param name="fostererId">ID of fosterer to remove.</param>
    /// <returns>True if a fosterer was removed otherwise false.</returns>
    public async Task<bool> DeleteByKeyAsync(int fostererId)
    {
        var rs = await httpClient.DeleteAsync($"{Route}/{fostererId}");
        return await rs.Content.ReadFromJsonAsync<bool>();
    }

    /// <summary>
    /// Get list of all fosterers in the database.
    /// </summary>
    /// <returns>List of fosterers, or empty list if none are found.</returns>
    public async Task<List<Fosterer>> GetAllAsync()
    {
        var rs = await httpClient.GetAsync($"{Route}");
        return await rs.Content.ReadFromJsonAsync<List<Fosterer>>() ?? [];
    }

    /// <summary>
    /// Get list of all fosterer names.
    /// </summary>
    /// <returns>List of fosterers, or empty list if none are found.</returns>
    public async Task<List<ListItem<int>>> GetAllNamesAsync()
    {
        var rs = await httpClient.GetAsync($"{Route}/names");
        return await rs.Content.ReadFromJsonAsync<List<ListItem<int>>>() ?? [];
    }

    
    /// <summary>
    /// Gets a single fosterer by their ID.
    /// </summary>
    /// <param name="fostererId">ID of fosterer to fetch</param>
    /// <returns>A single fosterer if found, otherwise null</returns>
    public async Task<Fosterer?> GetByKeyAsync(int fostererId)
        => await httpClient.GetFromJsonAsync<Fosterer>($"{Route}/{fostererId}");

    public async Task<Fosterer?> UpdateAsync(int fostererId, Fosterer fosterer)
    {
        var model = new FostererEditModel(fosterer);
        var rs = await httpClient.PutAsJsonAsync($"{Route}/{fostererId}", model);
        return await rs.Content.ReadFromJsonAsync<Fosterer>();
    }
}