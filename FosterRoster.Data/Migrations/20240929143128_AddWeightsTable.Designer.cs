﻿// <auto-generated />
using System;
using FosterRoster.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FosterRoster.Data.Migrations
{
    [DbContext(typeof(FosterRosterDbContext))]
    [Migration("20240929143128_AddWeightsTable")]
    partial class AddWeightsTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FosterRoster.Domain.Feline", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<string>("Breed")
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<int?>("IntakeAgeInWeeks")
                        .HasColumnType("integer");

                    b.Property<DateOnly>("IntakeDate")
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateOnly?>("RegistrationDate")
                        .HasColumnType("date");

                    b.Property<string>("Weaned")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.HasKey("Id");

                    b.ToTable("Felines", (string)null);
                });

            modelBuilder.Entity("FosterRoster.Domain.Thumbnail", b =>
                {
                    b.Property<int>("FelineId")
                        .HasColumnType("integer");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<byte[]>("ImageData")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<uint>("Version")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.HasKey("FelineId");

                    b.ToTable("Thumbnails", (string)null);
                });

            modelBuilder.Entity("Weight", b =>
                {
                    b.Property<int>("FelineId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("DateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Units")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(5, 2)");

                    b.HasKey("FelineId", "DateTime")
                        .HasName("PK_Weights");

                    b.ToTable("Weights", (string)null);
                });

            modelBuilder.Entity("FosterRoster.Domain.Thumbnail", b =>
                {
                    b.HasOne("FosterRoster.Domain.Feline", null)
                        .WithOne("Thumbnail")
                        .HasForeignKey("FosterRoster.Domain.Thumbnail", "FelineId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Weight", b =>
                {
                    b.HasOne("FosterRoster.Domain.Feline", null)
                        .WithMany("Weights")
                        .HasForeignKey("FelineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Weights_Felines");
                });

            modelBuilder.Entity("FosterRoster.Domain.Feline", b =>
                {
                    b.Navigation("Thumbnail");

                    b.Navigation("Weights");
                });
#pragma warning restore 612, 618
        }
    }
}
