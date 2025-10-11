using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace FosterRoster.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSterilizationDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cron",
                table: "Chores");

            migrationBuilder.DropColumn(
                name: "Repeats",
                table: "Chores");

            migrationBuilder.AddColumn<DateOnly>(
                name: "SterilizationDate",
                table: "Felines",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SterilizationDate",
                table: "Felines");

            migrationBuilder.AddColumn<string>(
                name: "Cron",
                table: "Chores",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Repeats",
                table: "Chores",
                type: "integer",
                nullable: false,
                defaultValue: 1);
        }
    }
}
