namespace FosterRoster.Services;

public sealed class ServerFostererRepository(
    IDbContextFactory<FosterRosterDbContext> contextFactory
) : IFostererRepository
{
    /// <summary>
    /// Adds a new fosterer to the database.
    /// </summary>
    /// <param name="fosterer">Fosterer instance to add.</param>
    /// <returns>Updated feline instance after add.</returns>
    public async Task<Fosterer> AddAsync(Fosterer fosterer)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var entry = await context.Fosterers.AddAsync(fosterer);
        await context.SaveChangesAsync();
        return entry.Entity;
    }

    /// <summary>
    /// Deletes a Fosterer by its ID.
    /// </summary>
    /// <param name="fostererId">ID of fosterer to remove.</param>
    /// <returns>True if a fosterer was removed otherwise false.</returns>
    public async Task<bool> DeleteByKeyAsync(int fostererId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context
            .Fosterers
            .Where(f => f.Id == fostererId)
            .ExecuteDeleteAsync() > 0;
    }

    public async Task<List<Fosterer>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context
            .Fosterers
            .AsNoTracking()
            .OrderBy(f => f.Name)
            .ToListAsync();
    }

    public async Task<List<ListItem<int>>> GetAllNamesAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context
            .Fosterers
            .AsNoTracking()
            .OrderBy(f => f.Name)
            .Select(f => new ListItem<int>(f.Id, f.Name))
            .ToListAsync();
    }

    public async Task<Fosterer?> GetByKeyAsync(int id)
    {
        await using var context = await contextFactory.CreateDbContextAsync();  
        return await context
            .Fosterers
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    /// <summary>
    /// Updates a Fosterer in the database.
    /// </summary>
    /// <param name="fostererId">ID of Fosterer to update</param>
    /// <param name="fosterer">Data to assign to Fosterer</param>
    /// <returns>Updated Fosterer if found, otherwise null</returns>
    public async Task<Fosterer?> UpdateAsync(int fostererId, Fosterer fosterer)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var existing = await context
            .Fosterers
            .FirstOrDefaultAsync(e => e.Id == fostererId);
        if (existing is null)
        {
            return null;
        }
        existing.Address = fosterer.Address;
        existing.ContactMethod = fosterer.ContactMethod;
        existing.Email = fosterer.Email; 
        existing.Phone = fosterer.Phone;
        existing.Name = fosterer.Name;
        
        await context.SaveChangesAsync();

        return existing;
    }
}