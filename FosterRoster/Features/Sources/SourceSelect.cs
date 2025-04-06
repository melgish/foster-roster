namespace FosterRoster.Features.Sources;

public class SourceSelect(SourceRepository sourceRepository) : AppItemSelect<int>
{
    private static readonly Item Select = new(0, "Select a source...");

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);

        if (Items is null)
        {
            // Get all choices from db.
            await using var query = await sourceRepository.CreateQueryAsync();
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