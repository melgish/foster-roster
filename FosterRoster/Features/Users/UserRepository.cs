using FosterRoster.Data;
using FosterRoster.Features.Fosterers;

namespace FosterRoster.Features.Users;

using Account;
using Microsoft.AspNetCore.Identity;

public sealed class UserRepository(IServiceScopeFactory scopeFactory) : IRepository
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
    public async Task<Result<IdOnlyDto>> AddAsync(UserFormDto dto)
    {
        // Need both Managers and EF context to create a user with fosterers.
        await using var scope = scopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FosterRosterDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        var userName = dto.UserName.TrimToNull();
        var email = dto.Email.TrimToNull();

        var user = new ApplicationUser
        {
            UserName = userName,
            Email = email,
            EmailConfirmed = true,
            PhoneNumber = dto.PhoneNumber.TrimToNull(),
            Fosterers = await dbContext.Fosterers.Where(f => dto.Fosterers.Contains(f.Id)).ToListAsync(),
            UserRoles = await roleManager.FindByNameAsync(dto.Role) is { } role 
                ? [new ApplicationUserRole() { Role = role }] : []
        };

        var rs = Map(await userManager.CreateAsync(user, dto.Password));
        
        return rs.ToResult(new IdOnlyDto(dto.Id));
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
    public async Task<Result<IdOnlyDto>> UpdateAsync(int userId, UserFormDto dto)
    {
        // Need both Managers and EF context to update a user with fosterers.
        await using var scope = scopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FosterRosterDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        var user = await userManager
            .Users
            .Include(u => u.Fosterers)
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
            return Result.Fail(new NotFoundError());

        user.Email = dto.Email.TrimToNull();
        user.PhoneNumber = dto.PhoneNumber.TrimToNull();

        // Update the role if changed.
        // Identity framework allows a user to be in multiple roles, but the
        // expectation for the application is that a user will be in a single
        // role.
        var role = await roleManager.FindByNameAsync(dto.Role);
        user.UserRoles = role is not null 
            ? [ new ApplicationUserRole { Role = role } ] 
            : [];

        // Update the fosterers.
        user.Fosterers = await dbContext
            .Fosterers
            .Where(f => dto.Fosterers.Contains(f.Id))
            .ToListAsync();

        // Save using the userManager to ensure any concurrency tokens are handled.
        var rs = Map(await userManager.UpdateAsync(user));
        if (rs.IsFailed)
            return rs;

        return rs.IsFailed ? rs : Result.Ok(new IdOnlyDto(user.Id));
    }
}