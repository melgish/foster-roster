namespace FosterRoster.Client.Services;

public sealed class ClientWeightRepository(
    HttpClient httpClient
) : IWeightRepository
{
    private const string Route = "api/weights";
    private const string FailedToCreate = "Failed to create weight";
    private const string FailedToDelete = "Failed to delete weight";

    /// <summary>
    /// Adds a new weight to the database for a given feline.
    /// </summary>
    /// <param name="weight">weight information about feline.</param>
    /// <returns>Result with Weight on success, or Errors on failure.</returns>
    public async Task<Result<Weight>> AddAsync(Weight weight)
        => await Result
            .Try(() => httpClient.PostAsJsonAsync<WeightEditModel>(Route, new(weight)))
            .Bind(rs => Result.OkIf(rs.IsSuccessStatusCode, FailedToCreate).ToResult(rs))
            .Bind(rs => Result.Try(() => rs.Content.ReadFromJsonAsync<Weight>()))
            .Bind(w => Result.OkIf(w is not null, FailedToCreate).ToResult(w!));

    /// <summary>
    /// Delete the given weight from the database.
    /// </summary>
    /// <param name="felineId">ID of feline.</param>
    /// <param name="dateTime">Date and Time of weight to remove.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> DeleteByKeyAsync(int felineId, DateTimeOffset dateTime)
    {
        var encoded = Uri.EscapeDataString(dateTime.ToString("o"));
        return await Result
            .Try(() => httpClient.DeleteAsync($"{Route}/{felineId}/{encoded}"))
            .Bind(rs => Result.OkIf(rs.IsSuccessStatusCode, FailedToDelete));
    }

    /// <summary>
    /// Get last 2 weeks of weights for all felines in the database.
    /// </summary>
    /// <returns>A Result with list of Weights success, or Errors on failure.</returns>
    public async Task<Result<List<Weight>>> GetAllAsync()
        => await Result
            .Try(() => httpClient.GetFromJsonAsync<List<Weight>>(Route))
            .Bind(list => Result.Ok(list!));
}