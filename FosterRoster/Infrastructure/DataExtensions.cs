namespace FosterRoster.Infrastructure;

using Data;

public static class DataExtensions
{
    /// <summary>
    ///     Adds a new entity to the supplied data set and saves the changes
    ///     to the database.
    /// </summary>
    /// <param name="dbContextFactory">Factory to create a database context</param>
    /// <param name="setFactory">Function to select set to modify.</param>
    /// <param name="entity">Entity to add</param>
    /// <typeparam name="TEntity">Type of entity to add</typeparam>
    /// <returns>Entity after save</returns>
    public static async Task<Result<TEntity>> AddAsync<TEntity>(
        this IDbContextFactory<FosterRosterDbContext> dbContextFactory,
        Func<FosterRosterDbContext, DbSet<TEntity>> setFactory,
        TEntity entity
    ) where TEntity : class
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var entry = await setFactory(db).AddAsync(entity);
        await db.SaveChangesAsync();
        return Result.Ok(entry.Entity);
    }
    
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
        return new(db, setFactory(db));
    }

    /// <summary>
    ///     Deletes an entity by its identifier
    /// </summary>
    /// <param name="dbContextFactory">Factory to create a database context</param>
    /// <param name="setFactory">Function to select set to modify.</param>
    /// <param name="id">Database identifier to select entity</param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static async Task<Result> DeleteByKeyAsync<TEntity>(
        this IDbContextFactory<FosterRosterDbContext> dbContextFactory,
        Func<FosterRosterDbContext, DbSet<TEntity>> setFactory,
        int id
    ) where TEntity : class, IKeyBearer
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await setFactory(db)
            .Where(e => e.Id == id)
            .ExecuteDeleteAsync() switch
        {
            0 => Result.Fail(new NotFoundError()),
            1 => Result.Ok(),
            _ => Result.Fail(new MultipleChangesError())
        };
    }
    
    /// <summary>
    ///     Get a single entity from the database using its ID
    /// </summary>
    /// <param name="dbContextFactory">Factory to create a database context</param>
    /// <param name="setFactory">Function to select set to modify.</param>
    /// <param name="id">Database identifier to select entity</param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static async Task<Result<TEntity>> GetByKeyAsync<TEntity>(
        this IDbContextFactory<FosterRosterDbContext> dbContextFactory,
        Func<FosterRosterDbContext, DbSet<TEntity>> setFactory,
        int id
    ) where TEntity : class, IKeyBearer
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await setFactory(db)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id) switch
        {
            null => Result.Fail(new NotFoundError()),
            { } entity => Result.Ok(entity)
        };
    }

    /// <summary>
    /// Updates an existing entity in the database
    /// </summary>
    /// <param name="dbContextFactory">Factory to create a database context</param>
    /// <param name="setFactory">Function to select set to modify.</param>
    /// <param name="id">Database identifier to select entity</param>
    /// <param name="mutator">Function to update entity properties</param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static async Task<Result<TEntity>> UpdateAsync<TEntity>(
        this IDbContextFactory<FosterRosterDbContext> dbContextFactory,
        Func<FosterRosterDbContext, DbSet<TEntity>> setFactory,
        int id,
        Func<TEntity, bool> mutator
    ) where TEntity : class, IKeyBearer
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var existing = await setFactory(db)
            .FirstOrDefaultAsync(e => e.Id == id);
        if (existing is null) return Result.Fail(new NotFoundError());
        if (mutator(existing))
        {
            await db.SaveChangesAsync();
        }
        return Result.Ok(existing);
    }
    
    /// <summary>
    /// Collapse strings to null if they are empty or whitespace.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string TrimToNull(this string? value)
        => string.IsNullOrWhiteSpace(value) ? null! : value.Trim();
    
    public static int? ZeroToNull(this int? value)
        => value == 0 ? null : value;

    public static int? ZeroToNull(this int value)
        => value == 0 ? null : value;
    
    
    
}