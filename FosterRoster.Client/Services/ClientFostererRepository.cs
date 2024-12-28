namespace FosterRoster.Client.Services;

public sealed class ClientFostererRepository(
    HttpClient httpClient
) : IFostererRepository
{
    private const string Route = "api/fosterers";
    private const string FailedToCreate = "Failed to create fosterer";
    private const string FailedToDelete = "Failed to delete fosterer";

    /// <summary>
    ///     Adds a new fosterer to the database.
    /// </summary>
    /// <param name="fosterer">Fosterer instance to add.</param>
    /// <returns>Result with Fosterer after add, or Errors on failure.</returns>
    public async Task<Result<Fosterer>> AddAsync(Fosterer fosterer)
        => await Result
            .Try(() => httpClient.PostAsJsonAsync(Route, new FostererEditModel(fosterer)))
            .Bind(rs => Result.OkIf(rs.IsSuccessStatusCode, FailedToCreate).ToResult(rs))
            .Bind(rs => Result.Try(() => rs.Content.ReadFromJsonAsync<Fosterer>()))
            .Bind(f => Result.OkIf(f is not null, FailedToCreate).ToResult(f!));

    /// <summary>
    ///     Deletes a Fosterer by its ID.
    /// </summary>
    /// <param name="fostererId">ID of fosterer to remove.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> DeleteByKeyAsync(int fostererId)
        => await Result
            .Try(() => httpClient.DeleteAsync($"{Route}/{fostererId}"))
            .Bind(rs => Result.OkIf(rs.IsSuccessStatusCode, FailedToDelete));

    /// <summary>
    ///     Gets list of all fosterers from the database.
    /// </summary>
    /// <returns>Result with list of fosterers, or Errors on failure.</returns>
    public async Task<Result<List<Fosterer>>> GetAllAsync()
        => await Result
            .Try(() => httpClient.GetFromJsonAsync<List<Fosterer>>(Route))
            .Bind(list => Result.Ok(list!));

    /// <summary>
    ///     Gets names of all fosterers from the database.
    /// </summary>
    /// <returns>Result with list of items, or Errors on failure.</returns>
    public async Task<Result<List<ListItem<int>>>> GetAllNamesAsync()
        => await Result
            .Try(() => httpClient.GetFromJsonAsync<List<ListItem<int>>>($"{Route}/names"))
            .Bind(list => Result.Ok(list!));

    /// <summary>
    ///     Gets single fosterer from the database.
    /// </summary>
    /// <param name="fostererId">ID of fosterer to return.</param>
    /// <returns>Result with Fosterer if successful, or Errors on failure.</returns>
    public async Task<Result<Fosterer>> GetByKeyAsync(int fostererId)
        => await Result
            .Try(() => httpClient.GetFromJsonAsync<Fosterer>($"{Route}/{fostererId}"))
            .Bind(fosterer => Result.Ok(fosterer!));

    public async Task<Result<Fosterer>> UpdateAsync(int fostererId, Fosterer fosterer)
    {
        var model = new FostererEditModel(fosterer);
        var rs = await httpClient.PutAsJsonAsync($"{Route}/{fostererId}", model);
        return Result.Ok((await rs.Content.ReadFromJsonAsync<Fosterer>())!);
    }
}