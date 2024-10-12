using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FosterRoster.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFelinesIsInactive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Value",
                table: "Weights",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,2)");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "InactivatedAtUtc",
                table: "Felines",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsInactive",
                table: "Felines",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Felines_IsInactive",
                table: "Felines",
                column: "IsInactive",
                filter: "\"IsInactive\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Felines_IsInactive",
                table: "Felines");

            migrationBuilder.DropColumn(
                name: "InactivatedAtUtc",
                table: "Felines");

            migrationBuilder.DropColumn(
                name: "IsInactive",
                table: "Felines");

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Weights",
                type: "numeric(5,2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");
        }
    }
}
