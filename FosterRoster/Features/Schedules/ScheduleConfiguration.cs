namespace FosterRoster.Features.Schedules;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.ToTable("Schedules");

        builder
            .HasKey(e => e.Id)
            .HasName("PK_Schedules");
        
        builder
            .Property(e => e.Cron)
            .HasMaxLength(128)
            .IsRequired();
        
        builder
            .Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        builder
            .Property(e => e.Name)
            .HasMaxLength(48)
            .IsRequired();

        // Cron must be unique in order to map schedule to a friendly name
        // without a hard relation.
        builder
            .HasIndex(e => e.Cron)
            .IsUnique()
            .HasDatabaseName("IX_Schedules_Cron");
    }
}