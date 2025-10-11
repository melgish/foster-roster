namespace FosterRoster.Features.Account;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
///     Database entity for application roles.
/// </summary>
public sealed class ApplicationRole : IdentityRole<int>, IIdBearer
{
    public ICollection<ApplicationUserRole> UserRoles { get; init; } = [];
}

/// <summary>
///     Database configuration for the <see cref="ApplicationRole"/> entity.
/// </summary>
[UsedImplicitly]
public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        // Add navigation property for Role to UserRoles
        // This needs to match the existing relation set up by the IdentityDbContext
        builder.HasMany(e => e.UserRoles)
            .WithOne(e => e.Role)
            .HasForeignKey(e => e.RoleId)
            .HasConstraintName("FK_AspNetUserRoles_AspNetRoles_RoleId")
            .IsRequired();
    }
}
