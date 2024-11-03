namespace FosterRoster.Client.Services;

public sealed class ClientSourceRepository(
    HttpClient httpClient
) : ISourceRepository
{
    const string Route = "api/sources";

    public async Task<List<Source>> GetAllAsync()
    {
        var rs = await httpClient.GetAsync($"{Route}");
        return await rs.Content.ReadFromJsonAsync<List<Source>>() ?? [];
    }

    public async Task<List<ListItem<int>>> GetAllNamesAsync()
    {
        var rs = await httpClient.GetAsync($"{Route}/names");
        return await rs.Content.ReadFromJsonAsync<List<ListItem<int>>>() ?? [];
    }
}