using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FosterRoster.Data.Migrations
{
    /// <inheritdoc />
    public partial class BiggerComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comments_FelineId_TimeStamp",
                table: "Comments");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "TimeStamp",
                table: "Comments",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Comments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_FelineId",
                table: "Comments",
                column: "FelineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comments_FelineId",
                table: "Comments");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "TimeStamp",
                table: "Comments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Comments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_FelineId_TimeStamp",
                table: "Comments",
                columns: new[] { "FelineId", "TimeStamp" },
                descending: new[] { false, true });
        }
    }
}
