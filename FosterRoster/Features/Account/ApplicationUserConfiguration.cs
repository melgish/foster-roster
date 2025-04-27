namespace FosterRoster.Features.Account;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

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