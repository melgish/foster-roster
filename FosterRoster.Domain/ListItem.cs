namespace FosterRoster.Domain;

/// <summary>
/// Represents any item that might be displayed in a list.
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <typeparam name="TValue"></typeparam>
public record ListItem<TValue>(TValue Id, [UsedImplicitly] string Name);