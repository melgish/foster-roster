namespace FosterRoster.Features.Chores;

public sealed class TemplateSelect(ChoreRepository choreRepository) : AppItemSelect<int>
{
    protected override async Task OnParametersSetAsync()
    {
        await using var query = await choreRepository.CreateQueryAsync();
        Items = await query
            .OnlyTemplates()
            .OrderBy(e => e.Name)
            .SelectToListItemDto()
            .ToListAsync();
    }
}