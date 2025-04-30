namespace FosterRoster.Infrastructure;

// Don't allow Dynamic to cover up EF Core's IQueryable
using Radzen;
using static Radzen.NotificationSeverity;
using Dynamic = System.Linq.Dynamic.Core.DynamicExtensions;

public static class RadzenExtensions
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
    /// Return value of ToGridResultsAsync
    /// </summary>
    /// <param name="Items"></param>
    /// <param name="Count"></param>
    /// <typeparam name="TEntity"></typeparam>
    public sealed record GridResults<TEntity>(List<TEntity> Items, int Count) where TEntity : class;

    /// <summary>
    ///     Run Radzen LoadDataArgs to apply filters and sorting to queryable, and fetch results.
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="args"></param>
    /// <param name="defaultOrderBy"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns>Results of query including total record count.</returns>
    public static async Task<GridResults<TEntity>> ToGridResultsAsync<TEntity>(
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
    ///     Open custom delete dialog and wait for true/false response
    /// </summary>
    /// <param name="dialogService"></param>
    /// <param name="entityId">ID of item to delete</param>
    /// <param name="name">Name to display in confirmation</param>
    /// <typeparam name="TComponent">Confirm component to display</typeparam>
    /// <returns></returns>
    public static async Task<bool> ConfirmDeleteAsync<TComponent>(
        this DialogService dialogService,
        int entityId,
        string name)
        where TComponent : ComponentBase, IConfirmDelete
        => entityId != 0 && await Convert.ToBoolean(
            await dialogService.OpenAsync<TComponent>(
                "Confirm Delete",
                new() { ["Name"] = name })
        );

    private static string? FirstReason<TReason>(this List<TReason> reasons) where TReason : IReason
        => reasons.FirstOrDefault()?.Message ?? string.Empty;

    /// <summary>
    ///     Notify user that grid failed to load any data.
    /// </summary>
    /// <param name="service"></param>
    /// <param name="ex"></param>
    public static void NotifyFailedToLoad(this NotificationService service, Exception ex)
        => service.Notify(Error, "Failed to load data", ex.GetBaseException().Message);

    /// <summary>
    ///     Notify user about success or failure of action.
    /// </summary>
    /// <param name="service">Notification service instance</param>
    /// <param name="result">Result to examine</param>
    /// <param name="target">Target of action</param>
    /// <param name="action">Action future tense</param>
    /// <param name="actioned">Action past tense</param>
    /// <returns></returns>
    public static bool NotifyResult(
        this NotificationService service,
        IResultBase result,
        string target,
        string action,
        string actioned)
    {
        if (result.IsSuccess)
        {
            service.Notify(Success, $"{target} {actioned}", result.Successes.FirstReason());
            return true;
        }

        service.Notify(Error, $"Failed to {action} {target}", result.Errors.FirstReason());
        return false;
    }
}