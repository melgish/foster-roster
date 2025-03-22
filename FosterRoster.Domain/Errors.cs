namespace FosterRoster.Domain;

/// <summary>
///     Error when primary entity in an operation is not found.
/// </summary>
/// <param name="message"></param>
public sealed class NotFoundError(string message = "Entity was not found") : Error(message);

/// <summary>
///     Error when operation results in 0 changes.
/// </summary>
/// <param name="message"></param>
public sealed class NoChangesError(string message = "Entity was not changed") : Error(message);

/// <summary>
///     Error when operation unexpectedly results in multiple changes.
/// </summary>
/// <param name="message"></param>
public sealed class MultipleChangesError(string message = "Multiple entities were changed") : Error(message);