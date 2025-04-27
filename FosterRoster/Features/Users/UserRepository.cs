namespace FosterRoster.Features.Users;

using Account;
using Microsoft.AspNetCore.Identity;

public sealed class UserRepository(IServiceScopeFactory scopeFactory)
{
    /// <summary>
    ///     Maps an IdentityResult to a FluentResults Result.
    /// </summary>
    /// <param name="result">Result to map</param>
    /// <returns></returns>
    private static Result Map(IdentityResult? result)
        => result switch
        {
            null => Result.Fail("Unexpected error"),
            { Succeeded: true } => Result.Ok(),
            { Errors: { } errors } => Result.Fail(string.Join(", ", errors.Select(e => e.Description)))
        };

    /// <summary>
    /// Adds a new user to the database in the supplied role.
    /// </summary>
    /// <param name="dto">Request information</param>
    /// <returns></returns>
    public async Task<Result<UserFormDto>> AddAsync(UserFormDto dto)
    {
        var userName = dto.UserName.TrimToNull();
        var email = dto.Email.TrimToNull();

        var user = new ApplicationUser
        {
            UserName = userName,
            Email = email,
            EmailConfirmed = true,
            PhoneNumber = dto.PhoneNumber.TrimToNull()
        };

        await using var scoped = scopeFactory.CreateScopedAsync<UserManager<ApplicationUser>>();

        var rs = Map(await scoped.Instance.CreateAsync(user, dto.Password));
        if (rs.IsSuccess && !string.IsNullOrWhiteSpace(dto.Role))
            rs = Map(await scoped.Instance.AddToRoleAsync(user, dto.Role));

        return rs.ToResult(dto);
    }

    /// <summary>
    ///     Captures a new database context and creates a queryable for the Weight table.
    /// </summary>
    /// <returns>Queryable with disposable context</returns>
    public Task<Query<ApplicationUser>> CreateQueryAsync()
        => scopeFactory.CreateQueryAsync<UserManager<ApplicationUser>, ApplicationUser>(db => db.Users.AsNoTracking());

    /// <summary>
    ///     Deletes the selected user from the database.
    /// </summary>
    /// <param name="userId">ID of user to delete.</param>
    /// <returns></returns>
    public async Task<Result> DeleteByKeyAsync(int userId)
    {
        await using var scoped = scopeFactory.CreateScopedAsync<UserManager<ApplicationUser>>();

        // UserManager will raise a concurrency exception if a proxy user
        // is passed to delete. Load the user from the database in order to 
        // delete it.
        return await scoped.Instance.FindByIdAsync(userId.ToString()) switch
        {
            { } user => Map(await scoped.Instance.DeleteAsync(user)),
            null => Result.Fail(new NotFoundError())
        };
    }

    /// <summary>
    ///     Gets single user from the database.
    /// </summary>
    /// <param name="userId">ID of user to return.</param>
    /// <returns>Result with Fosterer if successful, or Errors on failure.</returns>
    public async Task<Result<UserFormDto>> GetByKeyAsync(int userId)
    {
        await using var scoped = scopeFactory.CreateScopedAsync<UserManager<ApplicationUser>>();

        var dto = await scoped.Instance.Users
            .AsNoTracking()
            .SelectToFormDto()
            .FirstOrDefaultAsync(f => f.Id == userId);

        return dto is null
            ? Result.Fail(new NotFoundError())
            : Result.Ok(dto);
    }

    /// <summary>
    ///     Update an existing user in the database.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<Result<UserFormDto>> UpdateAsync(int userId, UserFormDto dto)
    {
        await using var scoped = scopeFactory.CreateScopedAsync<UserManager<ApplicationUser>>();

        var user = await scoped.Instance.FindByIdAsync(userId.ToString());
        if (user is null)
            return Result.Fail(new NotFoundError());

        user.Email = dto.Email.TrimToNull();
        user.PhoneNumber = dto.PhoneNumber.TrimToNull();

        var rs = Map(await scoped.Instance.UpdateAsync(user));
        if (rs.IsFailed)
            return rs;

        // Identity framework allows a user to be in multiple roles, but the
        // expectation for the application is that a user will be in a single
        // role.
        rs = (dto.Role.TrimToNull(), await scoped.Instance.GetRolesAsync(user)) switch
        {
            (null, []) => Result.Ok(),
            ({ } role, [var exists]) when exists == role => Result.Ok(),
            (null, { } roles) => Map(await scoped.Instance.RemoveFromRolesAsync(user, roles)),
            ({ } role, []) => Map(await scoped.Instance.AddToRoleAsync(user, role)),
            ({ } role, { } roles) => await Map(await scoped.Instance.RemoveFromRolesAsync(user, roles))
                .Bind(async Task<Result> () => Map(await scoped.Instance.AddToRoleAsync(user, role)))
        };
        if (rs.IsFailed)
            return rs;

        return Result.Ok(new UserFormDto
        {
            Email = user.Email ?? string.Empty,
            Id = user.Id,
            LockoutEnd = user.LockoutEnd,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            Role = (await scoped.Instance.GetRolesAsync(user)).FirstOrDefault() ?? string.Empty,
            UserName = user.UserName!
        });
    }
}