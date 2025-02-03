using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassedPawn.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemovedCoachIdFromCourseExercise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseExercises_Coaches_CoachId",
                table: "CourseExercises");

            migrationBuilder.DropIndex(
                name: "IX_CourseExercises_CoachId",
                table: "CourseExercises");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "CourseExercises");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoachId",
                table: "CourseExercises",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CourseExercises_CoachId",
                table: "CourseExercises",
                column: "CoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseExercises_Coaches_CoachId",
                table: "CourseExercises",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
