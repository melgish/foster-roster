namespace FosterRoster.Client.Services;

public sealed class ClientWeightRepository(
    HttpClient httpClient
) : IWeightRepository
{
    private const string Route = "api/weights";

    public async Task<Weight> AddAsync(Weight weight)
    {
        var rs = await httpClient.PostAsJsonAsync<WeightEditModel>(Route, new(weight));
        return await rs.Content.ReadFromJsonAsync<Weight>() ?? weight;
    }

    public async Task<bool> DeleteByKeyAsync(int felineId, DateTimeOffset dateTime)
    {
        var encoded = Uri.EscapeDataString(dateTime.ToString("o"));
        var rs = await httpClient.DeleteAsync($"{Route}/{felineId}/{encoded}");
        return await rs.Content.ReadFromJsonAsync<bool>();
    }

    /// <summary>
    /// Get list of all cats in the database.
    /// </summary>
    /// <returns>List of cats, or empty list if no cats exist.</returns>
    public async Task<List<Weight>> GetAllAsync()
        => await httpClient.GetFromJsonAsync<List<Weight>>(Route) ?? [];
}
