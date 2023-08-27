using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnagramSolver.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumnNameAndChangeIndexType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Celebrities_SortedName",
                table: "Celebrities");

            migrationBuilder.RenameColumn(
                name: "SortedName",
                table: "Celebrities",
                newName: "AnagramKey");

            migrationBuilder.CreateIndex(
                name: "IX_Celebrities_AnagramKey",
                table: "Celebrities",
                column: "AnagramKey")
                .Annotation("Npgsql:IndexMethod", "hash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Celebrities_AnagramKey",
                table: "Celebrities");

            migrationBuilder.RenameColumn(
                name: "AnagramKey",
                table: "Celebrities",
                newName: "SortedName");

            migrationBuilder.CreateIndex(
                name: "IX_Celebrities_SortedName",
                table: "Celebrities",
                column: "SortedName");
        }
    }
}
