using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;

#nullable disable

namespace FosterRoster.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFostererAndSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FostererId",
                table: "Felines",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                table: "Felines",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Fosterers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Address = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ContactMethod = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    Email = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    InactivatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsInactive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Phone = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fosterers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Felines_FostererId",
                table: "Felines",
                column: "FostererId");

            migrationBuilder.CreateIndex(
                name: "IX_Felines_SourceId",
                table: "Felines",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Fosterers_IsInactive",
                table: "Fosterers",
                column: "IsInactive",
                filter: "\"IsInactive\" = false");

            migrationBuilder.AddForeignKey(
                name: "FK_Fosterers_Felines",
                table: "Felines",
                column: "FostererId",
                principalTable: "Fosterers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Sources_Felines",
                table: "Felines",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.InsertData(
                table: "Sources",
                columns: ["Id", "Name"],
                values: new object[,]
                {
                    { 1, "City of Fort Worth" },
                    { 2, "Feral" },
                    { 3, "Citizen" },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fosterers_Felines",
                table: "Felines");

            migrationBuilder.DropForeignKey(
                name: "FK_Sources_Felines",
                table: "Felines");

            migrationBuilder.DropTable(
                name: "Fosterers");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropIndex(
                name: "IX_Felines_FostererId",
                table: "Felines");

            migrationBuilder.DropIndex(
                name: "IX_Felines_SourceId",
                table: "Felines");

            migrationBuilder.DropColumn(
                name: "FostererId",
                table: "Felines");

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "Felines");
        }
    }
}
