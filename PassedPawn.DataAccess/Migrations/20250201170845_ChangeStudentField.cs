using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassedPawn.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStudentField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PuzzleStudent_Students_StudentId",
                table: "PuzzleStudent");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "PuzzleStudent",
                newName: "StudentsId");

            migrationBuilder.RenameIndex(
                name: "IX_PuzzleStudent_StudentId",
                table: "PuzzleStudent",
                newName: "IX_PuzzleStudent_StudentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_PuzzleStudent_Students_StudentsId",
                table: "PuzzleStudent",
                column: "StudentsId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PuzzleStudent_Students_StudentsId",
                table: "PuzzleStudent");

            migrationBuilder.RenameColumn(
                name: "StudentsId",
                table: "PuzzleStudent",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_PuzzleStudent_StudentsId",
                table: "PuzzleStudent",
                newName: "IX_PuzzleStudent_StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PuzzleStudent_Students_StudentId",
                table: "PuzzleStudent",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
