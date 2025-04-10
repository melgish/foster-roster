using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FosterRoster.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAnimalId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ContactMethod",
                table: "Fosterers",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(16)",
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnimalId",
                table: "Felines",
                type: "character varying(24)",
                maxLength: 24,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnimalId",
                table: "Felines");

            migrationBuilder.AlterColumn<string>(
                name: "ContactMethod",
                table: "Fosterers",
                type: "character varying(16)",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(16)",
                oldMaxLength: 16);
        }
    }
}
