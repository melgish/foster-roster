using System.Web;

namespace FosterRoster.Client.Services;

internal static class HttpClientExtensions
{
    /// <summary>
    ///     Fetches data from the server using the supplied query parameters.
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="path"></param>
    /// <param name="filter"></param>
    /// <param name="top"></param>
    /// <param name="skip"></param>
    /// <param name="orderBy"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static async Task<Result<QueryResults<TEntity>>> QueryAsync<TEntity>(
        this HttpClient httpClient,
        string path,
        string? filter,
        int? top,
        int? skip,
        string? orderBy
    ) where TEntity : class
    {
        // Create the query string for the request
        var uri = new UriBuilder(httpClient.BaseAddress!) { Path = path };
        var query = HttpUtility.ParseQueryString(uri.Query);
        if (!string.IsNullOrWhiteSpace(filter)) query.Add(nameof(filter), filter);
        if (top.HasValue) query.Add(nameof(top), top.Value.ToString());
        if (skip.HasValue) query.Add(nameof(skip), skip.Value.ToString());
        if (!string.IsNullOrWhiteSpace(orderBy)) query.Add(nameof(orderBy), orderBy);
        uri.Query = query.ToString();

        return await Result
            .Try(() => httpClient.GetFromJsonAsync<QueryResults<TEntity>>(uri.Uri))
            .Bind((rs) => Result.OkIf(rs is not null, "Failed to load data").ToResult(rs!));
    }
}