namespace FosterRoster.Shared.Models;

public sealed record ListItemDto<TValue>(TValue Id, string Name);