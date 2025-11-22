namespace FosterRoster.Features.Users;

using Account;

public static class Queries
{
    extension(IQueryable<ApplicationUser> query)
    {
        /// <summary>
        ///     Map user entity to grid row model.
        /// </summary>
        /// <returns>Updated query that maps to grid row</returns>
        public IQueryable<UserGridDto> SelectToGridDto()
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
        /// <returns>Updated query that maps edit form model.</returns>
        public IQueryable<UserFormDto> SelectToFormDto()
            => query.Select(e => new UserFormDto
            {
                Email = e.Email ?? string.Empty,
                Id = e.Id,
                LockoutEnd = e.LockoutEnd,
                PhoneNumber = e.PhoneNumber ?? string.Empty,
                Role = e.UserRoles.Select(r => r.Role.Name).FirstOrDefault() ?? string.Empty,
                UserName = e.UserName ?? string.Empty,
                Password = string.Empty,
                ConfirmPassword = string.Empty,
                Fosterers = e.Fosterers.Select(f => f.Id).ToList()
            });
    }
}
