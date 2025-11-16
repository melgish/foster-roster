namespace FosterRoster.Infrastructure;

// Don't allow Dynamic to cover up EF Core's IQueryable
using Radzen;
using static Radzen.NotificationSeverity;
using Dynamic = System.Linq.Dynamic.Core.DynamicExtensions;

public static class RadzenExtensions
{
    /// <summary>
    ///     Return value of ToGridResultsAsync
    /// </summary>
    /// <param name="Items"></param>
    /// <param name="Count"></param>
    /// <typeparam name="TEntity"></typeparam>
    public sealed record GridResults<TEntity>(List<TEntity> Items, int Count) where TEntity : class;

    /// <param name="queryable"></param>
    /// <typeparam name="TEntity"></typeparam>
    extension<TEntity>(IQueryable<TEntity> queryable) where TEntity : class
    {
        /// <summary>
        ///     Applies Radzen Grid filtering to a queryable.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private IQueryable<TEntity> Where(LoadDataArgs args)
            => string.IsNullOrWhiteSpace(args.Filter) ? queryable : Dynamic.Where(queryable, args.Filter);

        /// <summary>
        ///     Applies Radzen Grid paging to a queryable.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private IQueryable<TEntity> Skip(LoadDataArgs args)
            => args.Skip.HasValue ? queryable.Skip(args.Skip.Value) : queryable;

        /// <summary>
        ///     Applies Radzen Grid paging to a queryable.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private IQueryable<TEntity> Take(LoadDataArgs args)
            => args.Top.HasValue ? queryable.Take(args.Top.Value) : queryable;

        /// <summary>
        ///     Applies Radzen Grid sorting to a queryable.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="defaultOrderBy"></param>
        /// <returns></returns>
        private IQueryable<TEntity> OrderBy(LoadDataArgs args,
            string? defaultOrderBy = null)
            => (string.IsNullOrWhiteSpace(args.OrderBy) ? defaultOrderBy : args.OrderBy)
                is { } orderBy
                    ? Dynamic.OrderBy(queryable, orderBy)
                    : queryable;

        /// <summary>
        ///     Run Radzen LoadDataArgs to apply filters and sorting to queryable, and fetch results.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="defaultOrderBy"></param>
        /// <returns>Results of query including total record count.</returns>
        public async Task<GridResults<TEntity>> ToGridResultsAsync(LoadDataArgs args,
            string? defaultOrderBy = null
        )
        {
            queryable = queryable.Where(args);
            var count = await queryable.CountAsync();
            var data = await queryable.OrderBy(args, defaultOrderBy).Skip(args).Take(args).ToListAsync();
            return new GridResults<TEntity>(data, count);
        }
    }

    extension(DialogService dialogService)
    {
        /// <summary>
        ///     Open custom delete dialog and wait for true/false response
        /// </summary>
        /// <param name="entityId">ID of item to delete</param>
        /// <param name="name">Name to display in confirmation</param>
        /// <typeparam name="TComponent">Confirm component to display</typeparam>
        /// <returns></returns>
        public async Task<bool> ConfirmDeleteAsync<TComponent>(int entityId,
            string name)
            where TComponent : ComponentBase, IConfirmDelete
            => entityId != 0 && Convert.ToBoolean(
                await dialogService.OpenAsync<TComponent>(
                    "Confirm Delete",
                    new Dictionary<string, object> { ["Name"] = name })
            );
    }

    extension<TReason>(IReadOnlyList<TReason> reasons) where TReason : IReason
    {
        private string FirstReason() =>
            reasons.Count > 0 ? reasons[0].Message : string.Empty;
    }

    /// <param name="service"></param>
    extension(NotificationService service)
    {
        /// <summary>
        ///     Notify user that grid failed to load any data.
        /// </summary>
        /// <param name="ex"></param>
        public void NotifyFailedToLoad(Exception ex)
            => service.Notify(Error, "Failed to load data", ex.GetBaseException().Message);

        /// <summary>
        ///     Notify user about success or failure of action.
        /// </summary>
        /// <param name="result">Result to examine</param>
        /// <param name="target">Target of action</param>
        /// <param name="action">Action future tense</param>
        /// <param name="actioned">Action past tense</param>
        /// <returns></returns>
        public bool NotifyResult(IResultBase result,
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
}