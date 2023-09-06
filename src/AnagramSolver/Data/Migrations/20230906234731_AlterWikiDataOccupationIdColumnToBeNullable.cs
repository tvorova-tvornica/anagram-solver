using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnagramSolver.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterWikiDataOccupationIdColumnToBeNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WikiDataOccupationId",
                table: "ImportWikiDataCelebritiesRequests",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WikiDataOccupationId",
                table: "ImportWikiDataCelebritiesRequests",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
