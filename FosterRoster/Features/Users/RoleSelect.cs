namespace FosterRoster.Features.Users;

public sealed class RoleSelect(
    IDbContextFactory<Data.FosterRosterDbContext> dbContextFactory
) : AppItemSelect<string>
{
    private static readonly ListItemDto<string> Select = new("", "Select a role...");

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);
        if (Items is null)
        {
            // Get all choices from db.
            await using var db = await dbContextFactory.CreateDbContextAsync();
            Items =
            [
                Select,
                .. await db
                    .Roles
                    .AsNoTracking()
                    .OrderBy(e => e.Name)
                    .Select(e => new ListItemDto<string>(e.Name!, e.Name!))
                    .ToListAsync()
            ];
        }

        await base.SetParametersAsync(ParameterView.Empty);
    }
}