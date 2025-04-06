namespace FosterRoster.Infrastructure;

public sealed record QueryResults<TEntity>(List<TEntity> Items, int Count) where TEntity : class;