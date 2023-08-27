using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnagramSolver.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixFullNameIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_Celebrities_FullName", table: "Celebrities");

            migrationBuilder.Sql(@"CREATE UNIQUE INDEX IX_Celebrities_FullName ON ""Celebrities"" (lower(""FullName""));");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_Celebrities_FullName", table: "Celebrities");

            migrationBuilder.CreateIndex(
                name: "IX_Celebrities_FullName",
                table: "Celebrities",
                column: "FullName",
                unique: true);
        }
    }
}
