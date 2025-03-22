namespace FosterRoster.Services;

// Don't allow Dynamic to cover up EF Core's IQueryable
using Radzen;
using Dynamic = System.Linq.Dynamic.Core.DynamicExtensions;

public static class RepositoryExtensions
{
    private static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> queryable, LoadDataArgs args)
        => string.IsNullOrWhiteSpace(args.Filter) ? queryable : Dynamic.Where(queryable, args.Filter);

    private static IQueryable<TEntity> Skip<TEntity>(this IQueryable<TEntity> queryable, LoadDataArgs args)
        => args.Skip.HasValue ? queryable.Skip(args.Skip.Value) : queryable;

    private static IQueryable<TEntity> Take<TEntity>(this IQueryable<TEntity> queryable, LoadDataArgs args)
        => args.Top.HasValue ? queryable.Take(args.Top.Value) : queryable;

    private static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> queryable, LoadDataArgs args,
        string? defaultOrderBy = null)
        => (string.IsNullOrWhiteSpace(args.OrderBy) ? defaultOrderBy : args.OrderBy)
            is { } orderBy
                ? Dynamic.OrderBy(queryable, orderBy)
                : queryable;

    /// <summary>
    ///     Run Radzen LoadDataArgs to apply filters and sorting to queryable, and fetch results.
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="args"></param>
    /// <param name="defaultOrderBy"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns>Results of query including total record count.</returns>
    public static async Task<QueryResults<TEntity>> ToQueryResultsAsync<TEntity>(
        this IQueryable<TEntity> queryable,
        LoadDataArgs args,
        string? defaultOrderBy = null
    ) where TEntity : class
    {
        queryable = queryable.Where(args);
        var count = await queryable.CountAsync();
        var data = await queryable.OrderBy(args, defaultOrderBy).Skip(args).Take(args).ToListAsync();
        return new(data, count);
    }
}