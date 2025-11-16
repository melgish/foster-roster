namespace FosterRoster.Infrastructure;

using Data;
using System.Collections;
using System.Linq.Expressions;

/// <summary>
/// A disposable wrapper around a DbSet.
/// A compromise between coupling repositories with Radzen or
/// directly exposing the database context inside components
/// </summary>
/// <param name="context"></param>
/// <param name="set"></param>
/// <typeparam name="TEntity"></typeparam>
public sealed class Query<TEntity>(object context, IQueryable<TEntity> set)
    : IQueryable<TEntity>, IDisposable, IAsyncDisposable
    where TEntity : class
{
    // IQueryable<T> implementation
    public IEnumerator<TEntity> GetEnumerator() => set.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)set).GetEnumerator();

    public Type ElementType => set.ElementType;

    public Expression Expression => set.Expression;

    public IQueryProvider Provider => set.Provider;

    // IDisposable implementation
    public void Dispose()
    {
        if (context is IDisposable disposable) disposable.Dispose();
    }

    // IAsyncDisposable implementation
    public async ValueTask DisposeAsync()
    {
        if (context is IAsyncDisposable asyncScope)
            await asyncScope.DisposeAsync();
        else
            Dispose();
    }
}

public static class Query
{
    /// <summary>
    /// Create a new queryable around the supplied DbSet.
    /// </summary>
    /// <param name="dbContextFactory">Factory to create a database context</param>
    /// <param name="setFactory">Function to select set to modify.</param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static async Task<Query<TEntity>> CreateQueryAsync<TEntity>(
        this IDbContextFactory<FosterRosterDbContext> dbContextFactory,
        Func<FosterRosterDbContext, IQueryable<TEntity>> setFactory
    ) where TEntity : class
    {
        var db = await dbContextFactory.CreateDbContextAsync();
        return new Query<TEntity>(db, setFactory(db));
    }

    /// <summary>
    /// Create a new queryable around a scoped service
    /// </summary>
    /// <param name="factory">Factory to create the service scope</param>
    /// <param name="setFactory">Factory to create the queryable from the service</param>
    /// <typeparam name="TService">Type of service to create</typeparam>
    /// <typeparam name="TEntity">Type of queryable entity</typeparam>
    /// <returns></returns>
    public static Task<Query<TEntity>> CreateQueryAsync<TService, TEntity>(
        this IServiceScopeFactory factory,
        Func<TService, IQueryable<TEntity>> setFactory
    ) where TService : notnull where TEntity : class
    {
        var scope = factory.CreateAsyncScope();
        var instance = scope.ServiceProvider.GetRequiredService<TService>();
        return Task.FromResult(new Query<TEntity>(scope, setFactory(instance)));
    }
}