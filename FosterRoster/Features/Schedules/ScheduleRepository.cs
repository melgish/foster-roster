namespace FosterRoster.Features.Schedules;

using Data;

public class ScheduleRepository(
    IDbContextFactory<FosterRosterDbContext> factory
)
{
    /// <summary>
    ///     Adds a new Schedule to the database.
    /// </summary>
    /// <param name="model">Schedule to add</param>
    /// <returns>A Result with Schedule if successful, or Errors on failure.</returns>
    public async Task<Result<Schedule>> AddAsync(ScheduleEditModel model)
    {
        await using var db = await factory.CreateDbContextAsync();
        var entry = await db.Schedules.AddAsync(new()
        {
            Cron = model.Cron.TrimToNull(),
            Name = model.Name.TrimToNull()
        });
        await db.SaveChangesAsync();
        return Result.Ok(entry.Entity);
    }
    
    /// <summary>
    ///     Captures a new database context and creates a queryable for the Schedule table.
    /// </summary>
    /// <returns></returns>
    public async Task<Query<Schedule>> CreateQueryAsync()
    {
        var db = await factory.CreateDbContextAsync();
        return new(db, db.Schedules);
    }
 
    /// <summary>
    ///     Deletes an existing Schedule from the database by its ID.
    /// </summary>
    /// <param name="scheduleId">ID of schedule to remove.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> DeleteByKeyAsync(int scheduleId)
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db.Schedules.Where(s => s.Id == scheduleId).ExecuteDeleteAsync() switch
        {
            0 => Result.Fail(new NotFoundError()),
            1 => Result.Ok(),
            _ => Result.Fail(new MultipleChangesError())
        };
    }
    
    /// <summary>
    ///     Gets an existing Schedule from the database by its ID.
    /// </summary>
    /// <param name="scheduleId">ID of schedule to fetch.</param>
    /// <returns>Result with Schedule if successful, or Errors on failure.</returns>
    public async Task<Result<Schedule>> GetByKeyAsync(int scheduleId)
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db
                .Schedules
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == scheduleId) switch
            {
                null => Result.Fail(new NotFoundError()),
                { } schedule => Result.Ok(schedule)
            };
    }
    
    /// <summary>
    ///     Updates an existing Schedule in the database.
    /// </summary>
    /// <param name="scheduleId">ID of schedule to update.</param>
    /// <param name="model">Data to assign to Schedule</param>
    /// <returns>Result with updated Schedule if found, or Errors on failure.</returns>
    public async Task<Result<Schedule>> UpdateAsync(int scheduleId, ScheduleEditModel model)
    {
        await using var db = await factory.CreateDbContextAsync();
        var existing = await db.Schedules.FindAsync(scheduleId);
        if (existing is null) return Result.Fail(new NotFoundError());

        existing.Cron = model.Cron.TrimToNull();
        existing.Name = model.Name.TrimToNull();
        
        await db.SaveChangesAsync();
        return Result.Ok(existing);
    }
}