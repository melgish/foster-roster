namespace FosterRoster.Data;

using FosterRoster.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class Commentonfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");

        builder.HasKey(e => e.Id);

        builder
            .Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        builder
            .Property(e => e.Text)
            .IsRequired(true)
            .HasColumnType("text")
            .HasMaxLength(2048);

        builder
            .Property(e => e.TimeStamp)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd();

        builder
            .HasIndex(e => new { e.FelineId, e.TimeStamp })
            .HasDatabaseName("IX_Comments_FelineId_TimeStamp")
            .IsDescending(false, true);
    }
}

