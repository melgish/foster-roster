namespace FosterRoster.Features.Chores;

using Comments;

public sealed class ChoreRepository(
    IDbContextFactory<Data.FosterRosterDbContext> dbContextFactory
)
{
    /// <summary>
    ///     Adds a new chore to the database.
    /// </summary>
    /// <param name="chore">Chore instance to add.</param>
    /// <returns>A Result with Chore on Success, otherwise Result with Errors.</returns>
    public async Task<Result<Chore>> AddAsync(Chore chore)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var entry = await context.Chores.AddAsync(chore);
        await context.SaveChangesAsync();
        return Result.Ok(entry.Entity);
    }
    
    /// <summary>
    /// Clone the specified template chore for the specified feline.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Result of clone containing ID of new record.</returns>
    public async Task<Result<int>> CloneTemplateAsync(CloneTemplateRequest request)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var template = await context
            .Chores
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.ChoreId && !c.FelineId.HasValue);
        if (template is null) return Result.Fail(new NotFoundError());

        var entry = context.Chores.Add(new()
            {
                Description = template.Description,
                FelineId = request.FelineId,
                Name = template.Name,
                Frequency = template.Frequency,
                Repeats = template.Repeats,
            });
        await context.SaveChangesAsync();
        
        return Result.Ok(entry.Entity.Id);
    }

    /// <summary>
    ///     Captures a new database context and creates a queryable for the Weight table.
    /// </summary>
    /// <returns></returns>
    public async Task<Query<Chore>> CreateQueryAsync()
    {
        var context = await dbContextFactory.CreateDbContextAsync();
        var queryable = context.Chores;
        return new(context, queryable);
    }

    /// <summary>
    ///     Deletes a Chore by its ID.
    /// </summary>
    /// <param name="choreId">ID of chore to remove.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> DeleteByKeyAsync(int choreId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        return await context.Chores.Where(s => s.Id == choreId).ExecuteDeleteAsync() switch
        {
            0 => Result.Fail(new NotFoundError()),
            1 => Result.Ok(),
            _ => Result.Fail(new MultipleChangesError())
        };
    }
    
    /// <summary>
    ///     Gets single fosterer from the database.
    /// </summary>
    /// <param name="choreId">ID of chore to return.</param>
    /// <returns>Result with Fosterer if successful, or Errors on failure.</returns>
    public async Task<Result<Chore>> GetByKeyAsync(int choreId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var chore = await db.Chores.AsNoTracking().FirstOrDefaultAsync(f => f.Id == choreId);
        return chore is null ? Result.Fail(new NotFoundError()) : Result.Ok(chore);
    }

    public async Task<Result> LogChoreCompletedAsync(int choreId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var chore = await db.Chores.FindAsync(choreId);
        if (chore is null) return Result.Fail(new NotFoundError());
        if (chore.FelineId is null) return Result.Fail("Task is not assigned to a feline.");
        if (chore.Repeats == 0) return Result.Fail("Task has already been completed.");
        
        // create journal entry for the chore.
        db.Comments.Add(new()
        {
            FelineId = chore.FelineId.GetValueOrDefault(),
            Text = string.IsNullOrWhiteSpace(chore.Description) ? chore.Name : chore.Description,
            TimeStamp = DateTimeOffset.UtcNow,
        });
        
        if (chore.Repeats == 1)
        {
            db.Chores.Remove(chore);
        }
        else
        {
            chore.Repeats -= 1;
            db.Chores.Update(chore);
        }
        
        await db.SaveChangesAsync();

        return Result.Ok();
    }

    /// <summary>
    ///     Updates an existing Chore in the database.
    /// </summary>
    /// <param name="choreId">ID of chore to update.</param>
    /// <param name="chore">Data to assign to Chore</param>
    /// <returns>Result with updated Chore if found, or Errors on failure.</returns>
    public async Task<Result<Chore>> UpdateAsync(int choreId, Chore chore)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var existing = await db.Chores.FindAsync(choreId);
        if (existing is null) return Result.Fail(new NotFoundError());

        existing.Description = chore.Description;
        existing.Name = chore.Name;

        await db.SaveChangesAsync();
        return Result.Ok(existing);
    }
}