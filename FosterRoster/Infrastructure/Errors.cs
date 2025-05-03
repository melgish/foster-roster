namespace FosterRoster.Infrastructure;

/// <summary>
///     Error when primary entity in an operation is not found.
/// </summary>
public sealed class NotFoundError() : Error("Entity was not found");

/// <summary>
///     Error when operation results in 0 changes.
/// </summary>
public sealed class NoChangesError() : Error("Entity was not changed");

/// <summary>
///     Error when operation unexpectedly results in multiple changes.
/// </summary>
public sealed class MultipleChangesError() : Error("Multiple entities were changed");