namespace FosterRoster.Features.Account;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
///     Database entity for application users.
/// </summary>
public sealed class ApplicationUser : IdentityUser<int>, IIdBearer
{
    public ICollection<ApplicationUserRole> UserRoles { get; init; } = [];
}

/// <summary>
///     Database configuration for the <see cref="ApplicationUser"/> entity.
/// </summary>
[UsedImplicitly]
public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Add navigation property for User to UserRoles
        // This needs to match the existing relation set up by the IdentityDbContext
        builder.HasMany(e => e.UserRoles)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .HasConstraintName("FK_AspNetUserRoles_AspNetUsers_UserId")
            .IsRequired();
    }
}