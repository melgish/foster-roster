using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;

#nullable disable

namespace FosterRoster.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVaccinationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vaccinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    AdministeredBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Comments = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ExpirationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    FelineId = table.Column<int>(type: "integer", nullable: false),
                    ManufacturerName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    SerialNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    VaccinationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    VaccineName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vaccinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vaccinations_Felines",
                        column: x => x.FelineId,
                        principalTable: "Felines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vaccinations_FelineId",
                table: "Vaccinations",
                column: "FelineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vaccinations");
        }
    }
}
