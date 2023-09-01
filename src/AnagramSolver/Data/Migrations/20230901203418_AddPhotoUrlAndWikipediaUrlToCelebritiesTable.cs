using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnagramSolver.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotoUrlAndWikipediaUrlToCelebritiesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Celebrities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WikipediaUrl",
                table: "Celebrities",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Celebrities");

            migrationBuilder.DropColumn(
                name: "WikipediaUrl",
                table: "Celebrities");
        }
    }
}
