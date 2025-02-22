namespace FosterRoster.Services;

// Don't allow Dynamic to cover up EF Core's IQueryable
using Dynamic = System.Linq.Dynamic.Core.DynamicExtensions;

public static class RepositoryExtensions
{
    /// <summary>
    /// Apply RadzenDataGrid style filters to the supplied queryable.
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="filter"></param>
    /// <param name="orderBy"></param>
    /// <param name="skip"></param>
    /// <param name="top"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static async Task<QueryResults<TEntity>> ToQueryResultsAsync<TEntity>(this IQueryable<TEntity> queryable,
        string? filter = null, int? top = null, int? skip = null, string? orderBy = null) where TEntity : class
    {
        queryable = string.IsNullOrWhiteSpace(filter) ? queryable : Dynamic.Where(queryable, filter);
        var count = await queryable.CountAsync();
        queryable = string.IsNullOrWhiteSpace(orderBy) ? queryable : Dynamic.OrderBy(queryable, orderBy);
        queryable = skip.HasValue ? queryable.Skip(skip.Value) : queryable;
        queryable = top.HasValue ? queryable.Take(top.Value) : queryable;

        var data = await queryable.ToListAsync();
        return new(data, count);
    }
}