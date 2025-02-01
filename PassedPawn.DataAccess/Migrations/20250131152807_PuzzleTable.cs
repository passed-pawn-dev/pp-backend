using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PassedPawn.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class PuzzleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Puzzle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Fen = table.Column<string>(type: "text", nullable: false),
                    CoachId = table.Column<int>(type: "integer", nullable: false),
                    Solution = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Puzzle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Puzzle_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PuzzleStudent",
                columns: table => new
                {
                    PuzzlesId = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PuzzleStudent", x => new { x.PuzzlesId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_PuzzleStudent_Puzzle_PuzzlesId",
                        column: x => x.PuzzlesId,
                        principalTable: "Puzzle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PuzzleStudent_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Puzzle_CoachId",
                table: "Puzzle",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_PuzzleStudent_StudentId",
                table: "PuzzleStudent",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PuzzleStudent");

            migrationBuilder.DropTable(
                name: "Puzzle");
        }
    }
}
