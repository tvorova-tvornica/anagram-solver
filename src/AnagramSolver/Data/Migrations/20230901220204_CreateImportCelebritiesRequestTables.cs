using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AnagramSolver.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateImportCelebritiesRequestTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImportWikiDataCelebritiesRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WikiDataOccupationId = table.Column<string>(type: "text", nullable: false),
                    WikiDataNationalityId = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportWikiDataCelebritiesRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImportWikiDataCelebritiesPageRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Offset = table.Column<int>(type: "integer", nullable: false),
                    Limit = table.Column<int>(type: "integer", nullable: false),
                    ImportCelebritiesRequestId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportWikiDataCelebritiesPageRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportWikiDataCelebritiesPageRequests_ImportWikiDataCelebri~",
                        column: x => x.ImportCelebritiesRequestId,
                        principalTable: "ImportWikiDataCelebritiesRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImportWikiDataCelebritiesPageRequests_ImportCelebritiesRequ~",
                table: "ImportWikiDataCelebritiesPageRequests",
                column: "ImportCelebritiesRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportWikiDataCelebritiesPageRequests");

            migrationBuilder.DropTable(
                name: "ImportWikiDataCelebritiesRequests");
        }
    }
}
