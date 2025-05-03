﻿namespace FosterRoster.Features.Users;

public class UserFormDto : IIdBearer
{
    /// <summary>
    ///     Password for new users or to change password for existing users.
    /// </summary>
    public string ConfirmPassword { get; set; } = string.Empty;

    /// <summary>
    ///     Unique ID for the user
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    ///     Unique name for the user. Must be an email address.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    ///     Email address of the user
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    ///     True if user has been locked out.
    /// </summary>
    public DateTimeOffset? LockoutEnd { [UsedImplicitly] get; set; }

    /// <summary>
    ///     Password for new users or to change password for existing users.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    ///     Optional phone number for the user
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    ///     A user can have one role.
    /// </summary>
    public string Role { get; set; } = string.Empty;
}