using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnagramSolver.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddHashIndexToHrAnagramKeyColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Celebrities_HrAnagramKey",
                table: "Celebrities",
                column: "HrAnagramKey")
                .Annotation("Npgsql:IndexMethod", "hash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Celebrities_HrAnagramKey",
                table: "Celebrities");
        }
    }
}
