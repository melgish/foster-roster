namespace FosterRoster.Features.Account;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

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