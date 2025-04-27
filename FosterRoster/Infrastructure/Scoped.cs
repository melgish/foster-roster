namespace FosterRoster.Infrastructure;

/// <summary>
/// A disposable wrapper around a scope and a service instance.
/// </summary>
/// <param name="scope">Scope that owns service instance</param>
/// <param name="instance">Service instance</param>
/// <typeparam name="TService">Type of instance</typeparam>
public class Scoped<TService>(object scope, TService instance) : IDisposable, IAsyncDisposable
{
    public TService Instance => instance;

    // IDisposable implementation
    public void Dispose()
    {
        if (scope is IDisposable disposable) disposable.Dispose();
    }

    // IAsyncDisposable implementation
    public async ValueTask DisposeAsync()
    {
        if (scope is IAsyncDisposable asyncScope)
            await asyncScope.DisposeAsync();
        else
            Dispose();
    }
}

public static class Scoped
{
    /// <summary>
    /// Create a new scope, and request the supplied service
    /// </summary>
    /// <param name="factory">Factory to create a service scope instance</param>
    /// <typeparam name="TService">Type of service to create inside the scope</typeparam>
    /// <returns>Disposable scope wrapper and service instance</returns>
    public static Scoped<TService> CreateScopedAsync<TService>(this IServiceScopeFactory factory)
        where TService : notnull
    {
        var scope = factory.CreateAsyncScope();
        var instance = scope.ServiceProvider.GetRequiredService<TService>();
        return new(scope, instance);
    }
}