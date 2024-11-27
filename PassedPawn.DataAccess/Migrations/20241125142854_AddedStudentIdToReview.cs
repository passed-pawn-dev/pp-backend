using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassedPawn.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedStudentIdToReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "CourseReviews",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CourseReviews_StudentId",
                table: "CourseReviews",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseReviews_Students_StudentId",
                table: "CourseReviews",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseReviews_Students_StudentId",
                table: "CourseReviews");

            migrationBuilder.DropIndex(
                name: "IX_CourseReviews_StudentId",
                table: "CourseReviews");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "CourseReviews");
        }
    }
}
