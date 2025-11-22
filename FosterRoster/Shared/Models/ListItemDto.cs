namespace FosterRoster.Shared.Models;

public sealed record ListItemDto<TValue>(
    [UsedImplicitly] TValue Id,
    [UsedImplicitly] string Name
);
