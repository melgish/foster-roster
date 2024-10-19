namespace FosterRoster.Data.Configurations;

using FosterRoster.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


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
            .IsRequired(true);

        builder
            .Property(e => e.DateTime)
            .IsRequired();

        builder
            .Property(e => e.Value)
            .HasColumnType("float")
            .IsRequired(true);

        builder
            .Property(e => e.Units)
            .HasConversion<string>()
            .IsRequired(true);
    }
}