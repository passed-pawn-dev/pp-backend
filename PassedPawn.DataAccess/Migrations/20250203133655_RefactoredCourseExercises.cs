using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PassedPawn.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RefactoredCourseExercises : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PuzzleStudent");

            migrationBuilder.DropTable(
                name: "Puzzle");

            migrationBuilder.RenameColumn(
                name: "Pgn",
                table: "CourseExercises",
                newName: "Solution");

            migrationBuilder.AddColumn<int>(
                name: "CoachId",
                table: "Courses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CourseExercises",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CoachId",
                table: "CourseExercises",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Fen",
                table: "CourseExercises",
                type: "text",
                nullable: false,
                defaultValue: "");

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
                name: "IX_Courses_CoachId",
                table: "Courses",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseExercises_CoachId",
                table: "CourseExercises",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseExerciseStudent_StudentsId",
                table: "CourseExerciseStudent",
                column: "StudentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseExercises_Coaches_CoachId",
                table: "CourseExercises",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Coaches_CoachId",
                table: "Courses",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseExercises_Coaches_CoachId",
                table: "CourseExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Coaches_CoachId",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "CourseExerciseStudent");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CoachId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_CourseExercises_CoachId",
                table: "CourseExercises");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "CourseExercises");

            migrationBuilder.DropColumn(
                name: "Fen",
                table: "CourseExercises");

            migrationBuilder.RenameColumn(
                name: "Solution",
                table: "CourseExercises",
                newName: "Pgn");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CourseExercises",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "Puzzle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CoachId = table.Column<int>(type: "integer", nullable: false),
                    Fen = table.Column<string>(type: "text", nullable: false),
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
                    StudentsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PuzzleStudent", x => new { x.PuzzlesId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_PuzzleStudent_Puzzle_PuzzlesId",
                        column: x => x.PuzzlesId,
                        principalTable: "Puzzle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PuzzleStudent_Students_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Puzzle_CoachId",
                table: "Puzzle",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_PuzzleStudent_StudentsId",
                table: "PuzzleStudent",
                column: "StudentsId");
        }
    }
}
