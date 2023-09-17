using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnagramSolver.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddHrFullNameAndHrAnagramKeyColumnsToCelebritiesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HrAnagramKey",
                table: "Celebrities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HrFullName",
                table: "Celebrities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OverrideOnNextWikiDataImport",
                table: "Celebrities",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HrAnagramKey",
                table: "Celebrities");

            migrationBuilder.DropColumn(
                name: "HrFullName",
                table: "Celebrities");

            migrationBuilder.DropColumn(
                name: "OverrideOnNextWikiDataImport",
                table: "Celebrities");
        }
    }
}
