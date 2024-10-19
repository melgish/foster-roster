namespace FosterRoster.Services;

using FosterRoster.Data;
using FosterRoster.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

public sealed class ServerWeightRepository(
    IDbContextFactory<FosterRosterDbContext> contextFactory
) : IWeightRepository
{

    private static readonly Expression<Func<Weight, Weight>> WeightProjection =
        w => new()
        {
            FelineId = w.FelineId,
            DateTime = w.DateTime,
            Value = w.Value,
            Units = w.Units,
            Feline = new()
            {
                Name = w.Feline.Name
            }
        };
    public async Task<Weight> AddAsync(Weight weight)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var entry = await context.AddAsync(weight);
        await context.SaveChangesAsync();
        // Need to fetch feline to return name.
        await entry.Reference(w => w.Feline).LoadAsync();
        return WeightProjection.Compile().Invoke(entry.Entity);
    }

    public async Task<bool> DeleteByKeyAsync(int felineId, DateTimeOffset dateTime)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var count = await context
            .Weights
            .Where(e => e.FelineId == felineId && e.DateTime == dateTime)
            .ExecuteDeleteAsync();
        return count > 0;
    }

    public async Task<List<Weight>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context
            .Felines
            .Include(f => f.Weights)
            .SelectMany(f => f
                .Weights
                .OrderByDescending(w => w.DateTime)
                .Select(w => new Weight
                {
                    FelineId = w.FelineId,
                    DateTime = w.DateTime,
                    Value = w.Value,
                    Units = w.Units,
                    Feline = new Feline { Name = f.Name }
                })
                .Take(14)
            )
            .OrderByDescending(w => w.DateTime)
            .ThenBy(w => w.Feline.Name)
            .ToListAsync();

    }
}