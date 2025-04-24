namespace FosterRoster.Features.Users;

using Account;
using Microsoft.AspNetCore.Identity;

public sealed class UserRepository(
    IDbContextFactory<Data.FosterRosterDbContext> dbContextFactory,
    IServiceScopeFactory scopeFactory
)
{
    public async Task<Result<UserFormDto>> AddAsync(UserFormDto dto)
    {
        var userName = dto.UserName.TrimToNull();
        var email = dto.Email.TrimToNull();
    
        var user = new ApplicationUser()
        {
            UserName = userName,
            Email = email,
            EmailConfirmed = true,
            PhoneNumber = dto.PhoneNumber.TrimToNull(),
        };
        
        using var scope = scopeFactory.CreateScope();
        using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var r0 = await userManager.CreateAsync(user, dto.Password);
        if (!r0.Succeeded)
            return Result.Fail<UserFormDto>(string.Join(", ", r0.Errors.Select(e => e.Description)));

        if (string.IsNullOrEmpty(dto.Role))
        {
            r0 = await userManager.AddToRoleAsync(user, dto.Role);
        }
        if (!r0.Succeeded)
        {
            return Result.Fail<UserFormDto>(string.Join(", ", r0.Errors.Select(e => e.Description)));
        }
        
        return await Task.FromResult(Result.Ok(dto));
    }
    
    /// <summary>
    ///     Captures a new database context and creates a queryable for the Weight table.
    /// </summary>
    /// <returns>Queryable with disposable context</returns>
    public Task<Query<ApplicationUser>> CreateQueryAsync()
        => dbContextFactory.CreateQueryAsync(db => db.Users);

    /// <summary>
    ///     Deletes the selected user from the database.
    /// </summary>
    /// <param name="userId">ID of user to delete.</param>
    /// <returns></returns>
    public async Task<Result> DeleteByKeyAsync(int userId)
    {
        using var scope = scopeFactory.CreateScope();
        using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        
        // It's necessary to load the user from the database to delete it.
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Result.Fail(new NotFoundError());
        
        var r0 = await userManager.DeleteAsync(user);
        return r0.Succeeded
            ? Result.Ok()
            : Result.Fail(string.Join(", ", r0.Errors.Select(e => e.Description)));
    }

    /// <summary>
    ///     Gets single user from the database.
    /// </summary>
    /// <param name="userId">ID of user to return.</param>
    /// <returns>Result with Fosterer if successful, or Errors on failure.</returns>
    public async Task<Result<UserFormDto>> GetByKeyAsync(int userId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var user = await db
            .Users
            .AsNoTracking()
            .Select(e => new UserFormDto
            {
                Email = e.Email ?? string.Empty,
                Id = e.Id,
                LockoutEnd = e.LockoutEnd,
                PhoneNumber = e.PhoneNumber ?? string.Empty,
                Role = string.Empty,
                UserName = e.UserName!,
            })
            .FirstOrDefaultAsync(f => f.Id == userId);

        if (user is null)
            return Result.Fail(new NotFoundError());

        // Identity does not create navigation properties for the roles.
        
        // TODO: Associate user with Fosterers via claims.
        
        return user switch {
            null => Result.Fail(new NotFoundError()),
            { } entity => Result.Ok(entity)
        };
    }
    
    public async Task<Result<UserFormDto>> UpdateAsync(int userId, UserFormDto dto)
    {
        await Task.CompletedTask;
        return Result.Ok(dto);
    }


}