using FosterRoster.Features.Fosterers;

namespace FosterRoster.Features.Account;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
///     Database entity for application users.
/// </summary>
public sealed class ApplicationUser : IdentityUser<int>, IIdBearer
{
    public List<ApplicationUserRole> UserRoles { get; set; } = [];
    
    public List<Fosterer> Fosterers { get; set; } = [];
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

        // Create many-to-many skip navigation between ApplicationUser and Fosterer
        // with join table info that's similar to rest of application.
        builder
            .HasMany(e => e.Fosterers)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "UserFosterers",
                j => j.HasOne<Fosterer>().WithMany().HasForeignKey("FostererId"),
                j => j.HasOne<ApplicationUser>().WithMany().HasForeignKey("UserId"),
                j => j.HasKey("UserId", "FostererId")
            );
    }
}