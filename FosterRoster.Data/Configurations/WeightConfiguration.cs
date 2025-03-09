using FosterRoster.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FosterRoster.Data.Configurations;

internal class WeightConfiguration : IEntityTypeConfiguration<Weight>
{
    public void Configure(EntityTypeBuilder<Weight> builder)
    {
        builder.ToTable("Weights");

        builder
            .HasKey(e => new { e.FelineId, e.DateTime })
            .HasName("PK_Weights");

        builder
            .Property(e => e.FelineId)
            .IsRequired();

        builder
            .Property(e => e.DateTime)
            .IsRequired();

        builder
            .Property(e => e.Value)
            .HasColumnType("float")
            .IsRequired();

        builder
            .Property(e => e.Units)
            .HasConversion<string>()
            .IsRequired();

        builder.HasQueryFilter(e => !e.Feline.IsInactive);
    }
}