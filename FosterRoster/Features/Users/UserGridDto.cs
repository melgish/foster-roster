namespace FosterRoster.Features.Users;

public sealed class UserGridDto
{
    /// <summary>
    ///     Email address of user
    /// </summary>
    public required string? Email { get; init; }

    /// <summary>
    ///     ID of user
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    ///     Phone Number of user
    /// </summary>
    public required string? PhoneNumber { get; init; }

    /// <summary>
    ///     UserName of user
    /// </summary>
    public required string UserName { get; init; }

    /// <summary>
    ///     Role of user
    /// </summary>
    public required string Role { get; init; }
}