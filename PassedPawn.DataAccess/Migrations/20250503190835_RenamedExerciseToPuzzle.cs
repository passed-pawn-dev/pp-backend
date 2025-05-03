using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PassedPawn.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenamedExerciseToPuzzle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseExerciseStudent");

            migrationBuilder.DropTable(
                name: "CourseExercises");

            migrationBuilder.CreateTable(
                name: "CoursePuzzles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Fen = table.Column<string>(type: "text", nullable: false),
                    Solution = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    LessonId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoursePuzzles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoursePuzzles_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoursePuzzleStudent",
                columns: table => new
                {
                    PuzzlesId = table.Column<int>(type: "integer", nullable: false),
                    StudentsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoursePuzzleStudent", x => new { x.PuzzlesId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_CoursePuzzleStudent_CoursePuzzles_PuzzlesId",
                        column: x => x.PuzzlesId,
                        principalTable: "CoursePuzzles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoursePuzzleStudent_Students_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoursePuzzles_LessonId",
                table: "CoursePuzzles",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursePuzzleStudent_StudentsId",
                table: "CoursePuzzleStudent",
                column: "StudentsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoursePuzzleStudent");

            migrationBuilder.DropTable(
                name: "CoursePuzzles");

            migrationBuilder.CreateTable(
                name: "CourseExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LessonId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Fen = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    Solution = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseExercises_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseExerciseStudent",
                columns: table => new
                {
                    PuzzlesId = table.Column<int>(type: "integer", nullable: false),
                    StudentsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseExerciseStudent", x => new { x.PuzzlesId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_CourseExerciseStudent_CourseExercises_PuzzlesId",
                        column: x => x.PuzzlesId,
                        principalTable: "CourseExercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseExerciseStudent_Students_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseExercises_LessonId",
                table: "CourseExercises",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseExerciseStudent_StudentsId",
                table: "CourseExerciseStudent",
                column: "StudentsId");
        }
    }
}
