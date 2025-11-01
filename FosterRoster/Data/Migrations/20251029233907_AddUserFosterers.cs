using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FosterRoster.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserFosterers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserFosterers",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    FostererId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFosterers", x => new { x.UserId, x.FostererId });
                    table.ForeignKey(
                        name: "FK_UserFosterers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFosterers_Fosterers_FostererId",
                        column: x => x.FostererId,
                        principalTable: "Fosterers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFosterers_FostererId",
                table: "UserFosterers",
                column: "FostererId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFosterers");
        }
    }
}
