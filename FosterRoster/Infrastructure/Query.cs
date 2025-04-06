namespace FosterRoster.Infrastructure;

using System.Collections;
using System.Linq.Expressions;

/// <summary>
/// A disposable wrapper around a DbSet.
/// A compromise between coupling repositories with Radzen or directly exposing the database context inside components
/// </summary>
/// <param name="context"></param>
/// <param name="set"></param>
/// <typeparam name="TEntity"></typeparam>
public sealed class Query<TEntity>(Data.FosterRosterDbContext context, DbSet<TEntity> set)
    : IQueryable<TEntity>, IDisposable, IAsyncDisposable
    where TEntity : class
{
    private Data.FosterRosterDbContext? _context = context;
    private readonly IQueryable<TEntity> _set = set;

    // IQueryable<T> implementation
    public IEnumerator<TEntity> GetEnumerator() => _set.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_set).GetEnumerator();

    public Type ElementType => _set.ElementType;

    public Expression Expression => _set.Expression;

    public IQueryProvider Provider => _set.Provider;

    // IDisposable implementation
    void IDisposable.Dispose()
    {
        if (_context is null) return;
        _context.Dispose();
        _context = null;
    }

    // IAsyncDisposable implementation
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (_context is not null)
        {
            await _context.DisposeAsync();
            _context = null;
        }
    }
}