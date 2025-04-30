namespace FosterRoster.Features.Users;

using Account;

public static class Mapping
{
    /// <summary>
    ///     Map user entity to grid row model.
    /// </summary>
    /// <param name="query">query instance to select from</param>
    /// <returns>Updated query that maps to grid row</returns>
    public static IQueryable<UserGridDto> SelectToGridDto(this IQueryable<ApplicationUser> query)
        => query.Select(e => new UserGridDto
        {
            Email = e.Email ?? string.Empty,
            Id = e.Id,
            PhoneNumber = e.PhoneNumber ?? string.Empty,
            Role = e.UserRoles.Select(r => r.Role.Name).FirstOrDefault() ?? string.Empty,
            UserName = e.UserName ?? string.Empty
        });

    /// <summary>
    ///     Map user entity to edit model.
    /// </summary>
    /// <param name="query">query instance to select from</param>
    /// <returns>Updated query that maps edit form model.</returns>
    public static IQueryable<UserFormDto> SelectToFormDto(this IQueryable<ApplicationUser> query)
        => query.Select(e => new UserFormDto
        {
            Email = e.Email ?? string.Empty,
            Id = e.Id,
            LockoutEnd = e.LockoutEnd,
            PhoneNumber = e.PhoneNumber ?? string.Empty,
            Role = e.UserRoles.Select(r => r.Role.Name).FirstOrDefault() ?? string.Empty,
            UserName = e.UserName ?? string.Empty,
            Password = string.Empty,
            ConfirmPassword = string.Empty
        });
}