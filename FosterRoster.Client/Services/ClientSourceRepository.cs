namespace FosterRoster.Client.Services;

public sealed class ClientSourceRepository(
    HttpClient httpClient
) : ISourceRepository
{
    private const string Route = "api/sources";
    private const string FailedToCreate = "Failed to create source";
    private const string FailedToDelete = "Failed to delete source";

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
    ///     Deletes a Source by its ID.
    /// </summary>
    /// <param name="sourceId">ID of source to remove.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> DeleteByKeyAsync(int sourceId)
        => await Result
            .Try(() => httpClient.DeleteAsync($"{Route}/{sourceId}"))
            .Bind(rs => Result.OkIf(rs.IsSuccessStatusCode, FailedToDelete));


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

    /// <summary>
    ///     Gets single Source from the database.
    /// </summary>
    /// <param name="sourceId">ID of source to return.</param>
    /// <returns>Result with Source if successful, or Errors on failure.</returns>
    public async Task<Result<Source>> GetByKeyAsync(int sourceId)
        => await Result
            .Try(() => httpClient.GetFromJsonAsync<Source>($"{Route}/{sourceId}"))
            .Bind(source => Result.Ok(source!));

    /// <summary>
    ///     Updates an existing Source in the database.
    /// </summary>
    /// <param name="sourceId">ID of Source to update</param>
    /// <param name="source">Data to assign to Source</param>
    /// <returns>Result with updated Source if found, or Errors on failure.</returns>
    public async Task<Result<Source>> UpdateAsync(int sourceId, Source source)
    {
        var model = new SourceEditModel(source);
        var rs = await httpClient.PutAsJsonAsync($"{Route}/{sourceId}", model);
        return Result.Ok((await rs.Content.ReadFromJsonAsync<Source>())!);
    }
}