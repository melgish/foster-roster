namespace FosterRoster.Features.Schedules;

using Data;
using System.Linq.Expressions;

public sealed class ScheduleRepository(
    IDbContextFactory<FosterRosterDbContext> dbContextFactory
)
{
    /// <summary>
    ///     Adds a new Schedule to the database.
    /// </summary>
    /// <param name="model">Schedule to add</param>
    /// <returns>A Result with Schedule if successful, or Errors on failure.</returns>
    public async Task<Result<ScheduleFormDto>> AddAsync(ScheduleFormDto model)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var entry = db.Schedules.Add(new()
        {
            Cron = model.Cron.TrimToNull(),
            Name = model.Name.TrimToNull()
        });
        await db.SaveChangesAsync();

        return Result.Ok(entry.Entity.ToFromDto());
    }

    /// <summary>
    ///     Captures a new database context and creates a queryable for the Schedule table.
    /// </summary>
    /// <returns></returns>
    public Task<Query<Schedule>> CreateQueryAsync()
        => dbContextFactory.CreateQueryAsync(db => db.Schedules.AsNoTracking());

    /// <summary>
    ///     Deletes an existing Schedule from the database by its ID.
    /// </summary>
    /// <param name="scheduleId">ID of schedule to remove.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> DeleteByKeyAsync(int scheduleId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await db
                .Schedules
                .Where(e => e.Id == scheduleId)
                .ExecuteDeleteAsync() switch
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
    public async Task<Result<ScheduleFormDto>> GetByKeyAsync(int scheduleId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var model = await db
            .Schedules
            .SelectToFormDto()
            .FirstOrDefaultAsync(e => e.Id == scheduleId);
        return model is null ? Result.Fail(new NotFoundError()) : Result.Ok(model);
    }

    /// <summary>
    ///     Updates an existing Schedule in the database.
    /// </summary>
    /// <param name="scheduleId">ID of schedule to update.</param>
    /// <param name="model">Data to assign to Schedule</param>
    /// <returns>Result with updated Schedule if found, or Errors on failure.</returns>
    public async Task<Result<ScheduleFormDto>> UpdateAsync(int scheduleId, ScheduleFormDto model)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();

        var schedule = await db.Schedules.FindAsync(scheduleId);
        if (schedule is null)
            return Result.Fail<ScheduleFormDto>(new NotFoundError());

        schedule.Cron = model.Cron.TrimToNull();
        schedule.Name = model.Name.TrimToNull();

        var entry = db.Schedules.Update(schedule);
        await db.SaveChangesAsync();

        return Result.Ok(entry.Entity.ToFromDto());
    }
}