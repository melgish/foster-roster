namespace FosterRoster.Domain;


public record ListItem<TValue>(TValue Value, string Text) { }