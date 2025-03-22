namespace FosterRoster.Components.Pages.Felines;

using Microsoft.AspNetCore.Components;
using Shared;

public class FostererSelect(
    IDbContextFactory<FosterRosterDbContext> dbContextFactory
) : AppItemSelect<int>
{
    private static readonly Item Select = new(0, "Select a fosterer...");

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);

        if (Items is null)
        {
            // Get all choices from db.
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            Items = await dbContext
                .Fosterers
                .AsNoTracking()
                .OrderBy(e => e.Name)
                .Select(e => new Item(e.Id, e.Name))
                .ToListAsync();
            Items = Items.Prepend(Select);
        }

        await base.SetParametersAsync(ParameterView.Empty);
    }
}