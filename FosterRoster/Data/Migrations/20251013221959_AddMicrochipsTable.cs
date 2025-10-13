using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FosterRoster.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMicrochipsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Microchips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Brand = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Comment = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    FelineId = table.Column<int>(type: "integer", nullable: false),
                    MicrochipId = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Microchips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Microchips_Felines_FelineId",
                        column: x => x.FelineId,
                        principalTable: "Felines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Microchips_FelineId",
                table: "Microchips",
                column: "FelineId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Microchips");
        }
    }
}
