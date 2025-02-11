namespace FosterRoster.Domain;

public sealed record QueryResults<TEntity>(List<TEntity> Items, int Count) where TEntity : class;