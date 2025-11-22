using FosterRoster.Data;
using FosterRoster.Features.Comments;

namespace FosterRoster.Features.Chores;

public sealed class ChoreRepository(
    IDbContextFactory<FosterRosterDbContext> factory
) : IRepository
{
    /// <summary>
    ///     Adds a new chore to the database.
    /// </summary>
    /// <param name="model">Chore data to add.</param>
    /// <returns>A Result with Chore on Success, otherwise Result with Errors.</returns>
    public async Task<Result<IdOnlyDto>> AddAsync(ChoreFormDto model)
    {
        await using FosterRosterDbContext db = await factory.CreateDbContextAsync();
        db.Chores.AddRange(model.FelineIds
            .Select(felineId => new Chore
            {
                Description = model.Description.TrimToNull(), DueDate = model.DueDate?.UtcDateTime, FelineId = felineId.ZeroToNull(), Name = model.Name.TrimToNull()
            }));
        await db.SaveChangesAsync();
        return Result.Ok(IdOnly.Zero);
    }

    /// <summary>
    ///     Captures a new database context and creates a queryable for the Weight table.
    /// </summary>
    /// <returns></returns>
    public async Task<Query<Chore>> CreateQueryAsync()
    {
        FosterRosterDbContext context = await factory.CreateDbContextAsync();
        DbSet<Chore> queryable = context.Chores;
        return new Query<Chore>(context, queryable);
    }

    /// <summary>
    ///     Deletes a Chore by its ID.
    /// </summary>
    /// <param name="choreId">ID of chore to remove.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> DeleteByKeyAsync(int choreId)
    {
        await using FosterRosterDbContext db = await factory.CreateDbContextAsync();
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
        await using FosterRosterDbContext db = await factory.CreateDbContextAsync();
        ChoreFormDto? dto = await db
            .Chores
            .Where(e => e.Id == choreId)
            .SelectToFormDto()
            .FirstOrDefaultAsync();
        return dto is null ? Result.Fail(new NotFoundError()) : Result.Ok(dto);
    }

    public async Task<Result> LogChoreCompletedAsync(int choreId, ChoreCompletionFormDto dto)
    {
        await using FosterRosterDbContext db = await factory.CreateDbContextAsync();
        Chore? chore = await db.Chores.FindAsync(choreId);
        if (chore is null)
        {
            return Result.Fail(new NotFoundError());
        }
        if (chore.FelineId is null)
        {
            return Result.Fail("Task is not assigned to a feline.");
        }

        db.Comments.Add(new Comment
        {
            FelineId = chore.FelineId.GetValueOrDefault(), Text = string.IsNullOrEmpty(dto.LogText) ? chore.Name : dto.LogText, TimeStamp = dto.LogDate!.Value.UtcDateTime
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
        await using FosterRosterDbContext db = await factory.CreateDbContextAsync();
        Chore? existing = await db.Chores.FindAsync(choreId);
        if (existing is null)
        {
            return Result.Fail(new NotFoundError());
        }

        existing.Description = model.Description;
        existing.DueDate = model.DueDate?.UtcDateTime;
        existing.Name = model.Name;
        existing.FelineId = model.FelineId;

        await db.SaveChangesAsync();
        return Result.Ok(existing.ToIdOnly());
    }
}
