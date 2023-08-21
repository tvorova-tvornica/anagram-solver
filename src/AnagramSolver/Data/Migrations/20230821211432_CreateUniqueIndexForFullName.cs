using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnagramSolver.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateUniqueIndexForFullName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Celebrities_FullName",
                table: "Celebrities",
                column: "FullName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Celebrities_FullName",
                table: "Celebrities");
        }
    }
}
