namespace FosterRoster.Client.Services;

public sealed class ClientFelineRepository(
    HttpClient httpClient
) : IFelineRepository
{
    private const string Route = "api/felines";
    private const string FailedToActivate = "Failed to activate feline";
    private const string FailedToCreate = "Failed to create feline";
    private const string FailedToDelete = "Failed to delete feline";
    private const string FailedToDeactivate = "Failed to deactivate feline";
    private const string FailedToSetThumbnail = "Failed to set thumbnail";

    /// <summary>
    ///     Restores identified feline to active status.
    /// </summary>
    /// <param name="felineId">ID of feline to update.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> ActivateAsync(int felineId)
        => await Result
            .Try(() => httpClient.PutAsync($"{Route}/{felineId}/activate", null))
            .Bind(rs => Result.OkIf(rs.IsSuccessStatusCode, FailedToActivate));

    /// <summary>
    ///     Adds a new feline to the database.
    /// </summary>
    /// <param name="feline">Feline instance to add.</param>
    /// <returns>A Result with added feline, or errors on failure.</returns>
    public async Task<Result<FelineEditModel>> AddAsync(FelineEditModel feline)
        => await Result
            .Try(() => httpClient.PostAsJsonAsync(Route, feline))
            .Bind(rs => Result.OkIf(rs.IsSuccessStatusCode, FailedToCreate).ToResult(rs))
            .Bind(rs => Result.Try(() => rs.Content.ReadFromJsonAsync<FelineEditModel>()))
            .Bind(f => Result.OkIf(f is not null, FailedToCreate).ToResult(f!));

    /// <summary>
    ///     Sets a feline as inactive in the database.
    /// </summary>
    /// <param name="felineId">ID of feline to deactivate.</param>
    /// <param name="dateTimeUtc">Date and Time of deactivation.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> DeactivateAsync(int felineId, DateTimeOffset dateTimeUtc)
    {
        var model = new DateTimeEditModel(dateTimeUtc.DateTime);
        var rs = await httpClient.PutAsJsonAsync($"{Route}/{felineId}/inactivate", model);
        return Result.OkIf(rs.IsSuccessStatusCode, FailedToDeactivate);
    }

    /// <summary>
    ///     Deletes a feline by its ID.
    /// </summary>
    /// <param name="felineId">ID of feline to remove.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> DeleteByKeyAsync(int felineId)
        => await Result
            .Try(() => httpClient.DeleteAsync($"{Route}/{felineId}"))
            .Bind(rs => Result.OkIf(rs.IsSuccessStatusCode, FailedToDelete));

    /// <summary>
    ///     Get list of all felines in the database.
    /// </summary>
    /// <returns>A Result with list of felines, or errors on failure.</returns>
    public async Task<Result<List<Feline>>> GetAllAsync()
        => await Result
            .Try(() => httpClient.GetFromJsonAsync<List<Feline>>(Route))
            .Bind(list => Result.Ok(list!));

    /// <summary>
    ///     Get names of all felines in the database, with only their names and ids.
    /// </summary>
    /// <returns>A Result with list of items, or errors on failure.</returns>
    public async Task<Result<List<ListItem<int>>>> GetAllNamesAsync()
        => await Result
            .Try(() => httpClient.GetFromJsonAsync<List<ListItem<int>>>($"{Route}/names"))
            .Bind(list => Result.Ok(list!));

    /// <summary>
    ///     Gets a single feline by ID.
    /// </summary>
    /// <param name="felineId">ID of feline to get.</param>
    /// <returns>A Result with Feline if found, or errors on failure</returns>
    public async Task<Result<Feline>> GetByKeyAsync(int felineId)
        => await Result
            .Try(() => httpClient.GetFromJsonAsync<Feline>($"{Route}/{felineId}"))
            .Bind(feline => Result.Ok(feline!));

    /// <summary>
    ///     Gets the thumbnail for a single feline.
    /// </summary>
    /// <param name="felineId">ID of the feline</param>
    /// <returns>A Result with Thumbnail if found, or errors on failure.</returns>
    public async Task<Result<Thumbnail>> GetThumbnailAsync(int felineId) =>
        await Result
            .Try(() => httpClient.GetFromJsonAsync<Thumbnail>($"{Route}/{felineId}/thumbnail"))
            .Bind(thumbnail => Result.Ok(thumbnail!));

    /// <summary>
    ///     Sets the thumbnail for a feline.
    /// </summary>
    /// <param name="felineId">ID of feline to change</param>
    /// <param name="thumbnail">Thumbnail to assign to feline</param>
    /// <returns>A Result with Feline if updated, or errors on failure.</returns>
    public async Task<Result<Feline>> SetThumbnailAsync(int felineId, Thumbnail thumbnail) =>
        await Result
            .Try(() => httpClient.PostAsJsonAsync($"{Route}/{felineId}/thumbnail", thumbnail))
            .Bind(rs => Result.OkIf(rs.IsSuccessStatusCode, FailedToSetThumbnail).ToResult(rs))
            .Bind(rs => Result.Try(() => rs.Content.ReadFromJsonAsync<Feline>()))
            .Bind(feline => Result.Ok(feline!));

    /// <summary>
    ///     Updates a feline in the database.
    /// </summary>
    /// <param name="felineId">ID of feline to update</param>
    /// <param name="feline">Data to assign to feline</param>
    /// <returns>A Result with Feline if updated, or errors on failure.</returns>
    public async Task<Result<Feline>> UpdateAsync(int felineId, Feline feline)
    {
        var model = new FelineEditModel(feline);
        var rs = await httpClient.PutAsJsonAsync($"{Route}/{felineId}", model);
        return Result.Ok((await rs.Content.ReadFromJsonAsync<Feline>())!);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="top"></param>
    /// <param name="skip"></param>
    /// <param name="orderBy"></param>
    /// <returns>A Result with data for Radzen Grid</returns>
    public async Task<Result<QueryResults<Feline>>> QueryAsync(string? filter, int? top, int? skip, string? orderBy)
        => await httpClient.QueryAsync<Feline>($"{Route}/query", filter, top, skip, orderBy);
}