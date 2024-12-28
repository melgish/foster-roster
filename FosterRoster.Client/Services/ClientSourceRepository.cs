namespace FosterRoster.Client.Services;

public sealed class ClientSourceRepository(
    HttpClient httpClient
) : ISourceRepository
{
    private const string Route = "api/sources";
    private const string FailedToCreate = "Failed to create source";

    /// <summary>
    ///     Adds a new source
    /// </summary>
    /// <param name="source">Source to add</param>
    /// <returns>A Result with Source if successful, or Errors on failure.</returns>
    public async Task<Result<Source>> AddAsync(Source source)
        => await Result
            .Try(() => httpClient.PostAsJsonAsync(Route, source))
            .Bind(rs => Result.OkIf(rs.IsSuccessStatusCode, FailedToCreate).ToResult(rs))
            .Bind(rs => Result.Try(() => rs.Content.ReadFromJsonAsync<Source>()))
            .Bind(s => Result.OkIf(s is not null, FailedToCreate).ToResult(s!));

    /// <summary>
    ///     Get list of all sources in the database.
    /// </summary>
    /// <returns>A Result with list of sources if successful, or Errors on failure.</returns>
    public async Task<Result<List<Source>>> GetAllAsync()
        => await Result
            .Try(() => httpClient.GetFromJsonAsync<List<Source>>($"{Route}"))
            .Bind(list => Result.Ok(list!));

    /// <summary>
    ///     Get list of all sources in the database, with only their names and ids.
    /// </summary>
    /// <returns>A Result with list of items if successful, or Errors on failure.</returns>
    public async Task<Result<List<ListItem<int>>>> GetAllNamesAsync()
        => await Result
            .Try(() => httpClient.GetFromJsonAsync<List<ListItem<int>>>($"{Route}/names"))
            .Bind((list) => Result.Ok(list!));
}