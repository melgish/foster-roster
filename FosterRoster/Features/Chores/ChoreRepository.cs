namespace FosterRoster.Features.Chores;

using Data;
using NCrontab;

public sealed class ChoreRepository(
    IDbContextFactory<FosterRosterDbContext> factory
)
{
    /// <summary>
    ///     Adds a new chore to the database.
    /// </summary>
    /// <param name="model">Chore data to add.</param>
    /// <returns>A Result with Chore on Success, otherwise Result with Errors.</returns>
    public async Task<Result<IdOnlyDto>> AddAsync(ChoreFormDto model)
    {
        await using var db = await factory.CreateDbContextAsync();
        await db.Chores.AddRangeAsync(model.FelineIds
            .Select(felineId => new Chore
            {
                Description = model.Description.TrimToNull(),
                DueDate = model.DueDate?.UtcDateTime,
                FelineId = felineId.ZeroToNull(),
                Name = model.Name.TrimToNull()
            }));
        await db.SaveChangesAsync();
        return Result.Ok(new IdOnlyDto(0));
    }

    /// <summary>
    ///     Captures a new database context and creates a queryable for the Weight table.
    /// </summary>
    /// <returns></returns>
    public async Task<Query<Chore>> CreateQueryAsync()
    {
        var context = await factory.CreateDbContextAsync();
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
        await using var db = await factory.CreateDbContextAsync();
        return await db.Chores.Where(e => e.Id == choreId).ExecuteDeleteAsync() switch
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
    public async Task<Result<ChoreFormDto>> GetByKeyAsync(int choreId)
    {
        await using var db = await factory.CreateDbContextAsync();
        var dto = await db
            .Chores
            .Where(e => e.Id == choreId)
            .SelectToFormDto()
            .FirstOrDefaultAsync();
        return dto is null ? Result.Fail(new NotFoundError()) : Result.Ok(dto);
    }

    /// <summary>
    ///     Creates journal entry that chore has been completed.
    /// </summary>
    /// <param name="choreId"></param>
    /// <returns>Result of operation</returns>
    public async Task<Result> LogChoreCompletedAsync(int choreId)
    {
        await using var db = await factory.CreateDbContextAsync();
        var chore = await db.Chores.FindAsync(choreId);
        if (chore is null) return Result.Fail(new NotFoundError());
        if (chore.FelineId is null) return Result.Fail("Task is not assigned to a feline.");

        // Create a journal entry for the chore.
        db.Comments.Add(new()
        {
            FelineId = chore.FelineId.GetValueOrDefault(),
            Text = string.IsNullOrWhiteSpace(chore.Description) ? chore.Name : chore.Description,
            TimeStamp = DateTimeOffset.UtcNow
        });

        db.Chores.Remove(chore);
        await db.SaveChangesAsync();
        return Result.Ok();
    }

    /// <summary>
    ///     Updates an existing Chore in the database.
    /// </summary>
    /// <param name="choreId">ID of chore to update.</param>
    /// <param name="model">Updated data to assign to Chore</param>
    /// <returns>Result with updated Chore if found, or Errors on failure.</returns>
    public async Task<Result<IdOnlyDto>> UpdateAsync(int choreId, ChoreFormDto model)
    {
        await using var db = await factory.CreateDbContextAsync();
        var existing = await db.Chores.FindAsync(choreId);
        if (existing is null) return Result.Fail(new NotFoundError());

        existing.Description = model.Description;
        existing.DueDate = model.DueDate?.UtcDateTime;
        existing.Name = model.Name;
        existing.FelineId = model.FelineId;

        await db.SaveChangesAsync();
        return Result.Ok(new IdOnlyDto(existing.Id));
    }
}