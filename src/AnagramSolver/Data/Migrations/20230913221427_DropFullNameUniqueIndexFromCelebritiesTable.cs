using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnagramSolver.Data.Migrations
{
    /// <inheritdoc />
    public partial class DropFullNameUniqueIndexFromCelebritiesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_celebrities_fullname",
                table: "Celebrities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Celebrities_FullName",
                table: "Celebrities",
                column: "FullName",
                unique: true);
        }
    }
}
