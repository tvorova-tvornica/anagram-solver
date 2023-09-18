using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnagramSolver.Data.Migrations
{
    /// <inheritdoc />
    public partial class DropOverrideOnNextWikiDataImportColumnFromCelebritiesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OverrideOnNextWikiDataImport",
                table: "Celebrities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OverrideOnNextWikiDataImport",
                table: "Celebrities",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
