using FosterRoster.Data;
using FosterRoster.Features.Felines;
using System.Linq.Expressions;

namespace FosterRoster.Features.Weights;

public sealed class WeightRepository(
    IDbContextFactory<FosterRosterDbContext> contextFactory
) : IRepository
{
    private static readonly Expression<Func<Weight, Weight>> WeightProjection =
        w => new Weight
        {
            FelineId = w.FelineId,
            DateTime = w.DateTime,
            Value = w.Value,
            Units = w.Units,
            Feline = new Feline
            {
                Name = w.Feline.Name
            }
        };

    /// <summary>
    ///     Adds a new weight to the database for a given feline.
    /// </summary>
    /// <param name="weight">weight information about feline.</param>
    /// <returns>Result with Weight on success, or Errors on failure.</returns>
    public async Task<Result<Weight>> AddAsync(Weight weight)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var entry = await context.AddAsync(weight);
        await context.SaveChangesAsync();
        // Need to fetch feline to return name.
        await entry.Reference(w => w.Feline).LoadAsync();
        return Result.Ok(WeightProjection.Compile().Invoke(entry.Entity));
    }

    /// <summary>
    ///     Captures a new database context and creates a queryable for the Weight table.
    /// </summary>
    /// <returns></returns>
    public async Task<Query<Weight>> CreateQueryAsync()
        => await contextFactory.CreateQueryAsync(db => db.Weights);

    /// <summary>
    ///     Delete the given weight from the database.
    /// </summary>
    /// <param name="felineId">ID of feline.</param>
    /// <param name="dateTime">Date and Time of weight to remove.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public async Task<Result> DeleteByKeyAsync(int felineId, DateTimeOffset dateTime)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context
                .Weights
                .Where(e => e.FelineId == felineId && e.DateTime == dateTime)
                .ExecuteDeleteAsync() switch
        {
            0 => Result.Fail(new NotFoundError()),
            1 => Result.Ok(),
            _ => Result.Fail(new MultipleChangesError())
        };
    }
}
