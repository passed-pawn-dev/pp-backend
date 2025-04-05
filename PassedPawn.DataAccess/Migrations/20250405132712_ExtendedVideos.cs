using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassedPawn.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedVideos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_CourseVideos_VideoId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_VideoId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "Lessons");

            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "CourseVideos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "CourseVideos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VideoPublicId",
                table: "CourseVideos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "CourseVideos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CourseVideos_LessonId",
                table: "CourseVideos",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseVideos_Lessons_LessonId",
                table: "CourseVideos",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseVideos_Lessons_LessonId",
                table: "CourseVideos");

            migrationBuilder.DropIndex(
                name: "IX_CourseVideos_LessonId",
                table: "CourseVideos");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "CourseVideos");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "CourseVideos");

            migrationBuilder.DropColumn(
                name: "VideoPublicId",
                table: "CourseVideos");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "CourseVideos");

            migrationBuilder.AddColumn<int>(
                name: "VideoId",
                table: "Lessons",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_VideoId",
                table: "Lessons",
                column: "VideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_CourseVideos_VideoId",
                table: "Lessons",
                column: "VideoId",
                principalTable: "CourseVideos",
                principalColumn: "Id");
        }
    }
}
