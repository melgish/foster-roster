namespace FosterRoster.Features.Fosterers;

public sealed class FostererSelect(FostererRepository fostererRepository) : AppItemSelect<int>
{
    private static readonly Item Select = new(0, "Select a fosterer...");

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);

        if (Items is null)
        {
            // Get all choices from db.
            await using var query = await fostererRepository.CreateQueryAsync();
            Items = await query
                .AsNoTracking()
                .OrderBy(e => e.Name)
                .Select(e => new Item(e.Id, e.Name))
                .ToListAsync();
            Items = Items.Prepend(Select);
        }

        await base.SetParametersAsync(ParameterView.Empty);
    }
}