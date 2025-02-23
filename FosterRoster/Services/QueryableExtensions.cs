namespace FosterRoster.Services;

// Don't allow Dynamic to cover up EF Core's IQueryable
using Radzen;
using Dynamic = System.Linq.Dynamic.Core.DynamicExtensions;

public static class RepositoryExtensions
{
    public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> queryable, LoadDataArgs args)
        => string.IsNullOrWhiteSpace(args.Filter) ? queryable : Dynamic.Where(queryable, args.Filter);

    public static IQueryable<TEntity> Skip<TEntity>(this IQueryable<TEntity> queryable, LoadDataArgs args)
        => args.Skip.HasValue ? queryable.Skip(args.Skip.Value) : queryable;
    
    public static IQueryable<TEntity> Take<TEntity>(this IQueryable<TEntity> queryable, LoadDataArgs args)
        => args.Top.HasValue ? queryable.Take(args.Top.Value) : queryable;

    public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> queryable, LoadDataArgs args, string? defaultOrderBy = null)
        => (string.IsNullOrWhiteSpace(args.OrderBy) ? defaultOrderBy : args.OrderBy) 
            is {} orderBy ? Dynamic.OrderBy(queryable, orderBy) : queryable;

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