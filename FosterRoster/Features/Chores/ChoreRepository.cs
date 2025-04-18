namespace FosterRoster.Features.Chores;

using Data;
using NCrontab;

public sealed class ChoreRepository(
    IDbContextFactory<FosterRosterDbContext> factory,
    TimeProvider timeProvider
)
{
    /// <summary>
    ///     Adds a new chore to the database.
    /// </summary>
    /// <param name="model">Chore data to add.</param>
    /// <returns>A Result with Chore on Success, otherwise Result with Errors.</returns>
    public async Task<Result<Chore>> AddAsync(ChoreEditModel model)
    {
        await using var db = await factory.CreateDbContextAsync();
        var entry = await db.Chores.AddAsync(new()
        {
            Cron = model.Cron.TrimToNull(),
            Description = model.Description.TrimToNull(),
            DueDate = model.DueDate?.UtcDateTime,
            FelineId = model.FelineId.ZeroToNull(),
            Name = model.Name.TrimToNull(),
            Repeats = model.Repeats,
        });
        await db.SaveChangesAsync();
        return Result.Ok(entry.Entity);
    }
    
    private DateTimeOffset? GetNextDueDate(DateTimeOffset? dueDate, string? cron)
    {
        if (dueDate is null || string.IsNullOrWhiteSpace(cron)) return dueDate;
        var schedule = CrontabSchedule.Parse(cron);
        var now = timeProvider.GetLocalNow().LocalDateTime;
        var nextDue = schedule.GetNextOccurrence(now);
        return nextDue;
    }
    
    /// <summary>
    /// Clone the specified template chore for the specified feline.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Result of clone containing ID of new record.</returns>
    public async Task<Result<int>> CloneTemplateAsync(CloneTemplateRequest request)
    {
        await using var context = await factory.CreateDbContextAsync();
        var template = await context
            .Chores
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.ChoreId && !c.FelineId.HasValue);
        if (template is null) return Result.Fail(new NotFoundError());

        var entry = context.Chores.Add(new()
            {
                Description = template.Description,
                DueDate = GetNextDueDate(template.DueDate, template.Cron)?.UtcDateTime,
                FelineId = request.FelineId,
                Name = template.Name,
                Cron = template.Cron,
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
        return await db.Chores.Where(s => s.Id == choreId).ExecuteDeleteAsync() switch
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
        await using var db = await factory.CreateDbContextAsync();
        var chore = await db.Chores.AsNoTracking().FirstOrDefaultAsync(f => f.Id == choreId);
        return chore is null ? Result.Fail(new NotFoundError()) : Result.Ok(chore);
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
        if (chore.Repeats == 0) return Result.Fail("Task has already been completed.");
        
        // Create a journal entry for the chore.
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
            chore.DueDate = GetNextDueDate(chore.DueDate, chore.Cron)?.UtcDateTime;
            db.Chores.Update(chore);
        }
        
        await db.SaveChangesAsync();

        return Result.Ok();
    }

    /// <summary>
    ///     Updates an existing Chore in the database.
    /// </summary>
    /// <param name="choreId">ID of chore to update.</param>
    /// <param name="model">Updated data to assign to Chore</param>
    /// <returns>Result with updated Chore if found, or Errors on failure.</returns>
    public async Task<Result<Chore>> UpdateAsync(int choreId, ChoreEditModel model)
    {
        await using var db = await factory.CreateDbContextAsync();
        var existing = await db.Chores.FindAsync(choreId);
        if (existing is null) return Result.Fail(new NotFoundError());

        existing.Cron = model.Cron;
        existing.Description = model.Description;
        existing.DueDate = model.DueDate?.UtcDateTime;
        existing.Name = model.Name;
        // existing.FelineId = model.FelineId;
        existing.Repeats = model.Repeats;

        await db.SaveChangesAsync();
        return Result.Ok(existing);
    }
}
