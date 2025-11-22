namespace FosterRoster.Features.Sources;

public class SourceSelect(SourceRepository sourceRepository) : AppItemSelect<int>
{
    private static readonly ListItemDto<int> Select = new(0, "Select a source...");

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);

        if (Items is null)
        {
            // Get all choices from db.
            await using var query = await sourceRepository.CreateQueryAsync();
            Items =
            [
                Select,
                .. await query
                    .OrderBy(e => e.Name)
                    .SelectToListItemDto()
                    .ToListAsync()
            ];
        }

        await base.SetParametersAsync(ParameterView.Empty);
    }
}
